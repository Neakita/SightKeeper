using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using CommunityToolkit.Diagnostics;
using Serilog;

namespace SightKeeper.Application.Training.RFDETR;

internal sealed class RFDETROutputParser(ILogger logger) : OutputParser
{
	public IObservable<EpochResult> Parse(IObservable<string> output)
	{
		return Observable.Create<EpochResult>(async (observer, cancellationToken) =>
		{
			var channel = Channel.CreateUnbounded<string>(_channelOptions);
			using var subscription = output
				.Select(channel.Writer.TryWrite)
				.Subscribe(isWritten => Guard.IsTrue(isWritten));
			do
			{
				var result = await ParseEpochAsync(channel, cancellationToken);
				logger.Verbose("Sending epoch result");
				observer.OnNext(result);
			} while (!await IsTrainingEndAsync(channel.Reader, cancellationToken));
			observer.OnCompleted();
			logger.Debug("Parsing completed");
		});
	}

	private readonly UnboundedChannelOptions _channelOptions = new()
	{
		SingleReader = true,
		SingleWriter = true
	};

	private async Task<EpochResult> ParseEpochAsync(Channel<string> channel, CancellationToken cancellationToken)
	{
		logger.Verbose("Searching for epoch start");
		var nextEpochStart = await ReadFirstAsync(channel.Reader, IsEpochStart, cancellationToken);
		logger.Verbose("Considered as epoch start: {line}", nextEpochStart);

		logger.Verbose("Searching for epoch total time");
		var epochTotalTime = await ReadFirstAsync(channel.Reader, IsEpochTotalTime, cancellationToken);
		logger.Verbose("Considered as epoch total time: {line}", epochTotalTime);

		var epochNumber = GetEpochNumber(epochTotalTime);
		logger.Verbose("Considered as epoch number: {epochNumber}", epochNumber);

		var averagedTrainStats = channel.Reader.ReadAsync(cancellationToken);
		logger.Verbose("Considered as averaged train stats: {line}", averagedTrainStats);

		logger.Verbose("Searching for test total time");
		var regularTestTotalTime = await ReadFirstAsync(channel.Reader, IsTestTotalTime, cancellationToken);
		logger.Verbose("Considered as regular test total time: {line}", regularTestTotalTime);

		var regularAveragedTestStats = await channel.Reader.ReadAsync(cancellationToken);
		logger.Verbose("Considered as regular averaged test stats: {line}", regularAveragedTestStats);

		logger.Verbose("Searching for test total time");
		var emaTestTotalTime = await ReadFirstAsync(channel.Reader, IsTestTotalTime, cancellationToken);
		logger.Verbose("Considered as ema test total time: {line}", emaTestTotalTime);

		var emaAveragedTestStats = await channel.Reader.ReadAsync(cancellationToken);
		logger.Verbose("Considered as ema averaged test starts: {line}", emaAveragedTestStats);

		return new EpochResult
		{
			EpochNumber = epochNumber
		};
	}

	private readonly Regex _trainTotalTimeRegex = new(@"^Epoch: \[\d+] \S.*$");
	private readonly Regex _numberRegex = new(@"\d+");

	private static async Task<string> ReadFirstAsync(
		ChannelReader<string> reader,
		Func<string, bool> predicate,
		CancellationToken cancellationToken)
	{
		string line;
		do line = await reader.ReadAsync(cancellationToken);
		while (!predicate(line));
		return line;
	}

	private bool IsEpochTotalTime(string line)
	{
		return _trainTotalTimeRegex.IsMatch(line);
	}

	private int GetEpochNumber(string epochTotalTimeLine)
	{
		var match = _numberRegex.Match(epochTotalTimeLine);
		return int.Parse(match.Value);
	}

	private static bool IsTestTotalTime(string line)
	{
		return line.StartsWith("Test: T");
	}

	private static bool IsEpochStart(string line)
	{
		return line.StartsWith("LENGTH OF DATA LOADER: ");
	}

	private async Task<bool> IsTrainingEndAsync(ChannelReader<string> reader, CancellationToken cancellationToken)
	{
		logger.Verbose("Search for last metric");
		var lastMetric = await ReadFirstAsync(reader, IsLastMetric, cancellationToken);
		logger.Verbose("Considered as last metric: {line}", lastMetric);
		var potentialTrainingTime = await reader.ReadAsync(cancellationToken);
		if (!potentialTrainingTime.StartsWith("Training time "))
			return false;
		logger.Verbose("Considered as training time: {line}", potentialTrainingTime);
		return true;
	}

	private static bool IsLastMetric(string line)
	{
		return line.StartsWith(" Average Recall     (AR) @[ IoU=0.50:0.95 | area= large | maxDets=100 ] = ");
	}
}