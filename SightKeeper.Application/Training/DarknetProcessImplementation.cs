using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Serilog;

namespace SightKeeper.Application.Training;

public sealed class DarknetProcessImplementation : DarknetProcess, IDisposable
{
	public IObservable<string> OutputReceived => _outputReceived.AsObservable();

	public DarknetProcessImplementation(string darknetExecutablePath, ILogger? logger = null)
	{
		_darknetExecutablePath = darknetExecutablePath;
		_logger = logger == null
			? Log.Logger
			: new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Logger(Log.Logger).WriteTo.Logger(logger).CreateLogger();
	}
	public async Task RunAsync(DarknetArguments arguments, CancellationToken cancellationToken = default)
	{
		var process = RunProcess(arguments);
		try
		{
			await process.WaitForExitAsync(cancellationToken);
		}
		catch (OperationCanceledException)
		{
		}
		finally
		{
			if (!process.HasExited) Close(process);
			Complete(process);
		}
	}

	private Process RunProcess(DarknetArguments arguments)
	{
		ProcessStartInfo processStartInfo = new(_darknetExecutablePath, arguments.ToString())
		{
			WorkingDirectory = DarknetHelper.DarknetDirectory,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			UseShellExecute = false,
			CreateNoWindow = arguments.DoNotShow
		};
		Process process = new();
		process.StartInfo = processStartInfo;
		process.OutputDataReceived += OnOutputDataReceived;
		process.ErrorDataReceived += OnOutputDataReceived;
		process.Start();
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();
		return process;
	}

	private void Complete(Process process)
	{
		process.OutputDataReceived -= OnOutputDataReceived;
		process.Dispose();
		_outputReceived.OnCompleted();
	}

	private void Close(Process process)
	{
		process.Close();
		if (process.WaitForExit(1000)) return;
		_logger.Warning("Process did not exit in time, terminating...");
		process.Kill();
		_logger.Information("Process terminated");
	}
	
	public void Dispose()
	{
		_outputReceived.Dispose();
	}
	
	private readonly Subject<string> _outputReceived = new();
	private readonly string _darknetExecutablePath;
	private readonly ILogger _logger;

	private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
	{
		if (e.Data != null)
		{
			_logger.Verbose("Received output from darknet: {Data}", e.Data);
			_outputReceived.OnNext(e.Data);
		}
		else _logger.Warning("Received null output from darknet");
	}
}