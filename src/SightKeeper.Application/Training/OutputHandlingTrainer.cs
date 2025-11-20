using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using Serilog;
using SightKeeper.Application.Misc;
using SightKeeper.Application.Training.RFDETR;

namespace SightKeeper.Application.Training;

internal sealed class OutputHandlingTrainer(
	Trainer trainer,
	OutputParser outputParser,
	IObserver<object> progressObserver,
	ILogger logger)
	: Trainer
{
	public async Task TrainAsync(CancellationToken cancellationToken)
	{
		using var outputHandling = SetupOutputHandling();
		await trainer.TrainAsync(cancellationToken);
	}

	private RemainingTimeEstimator? _timeEstimator;

	private IDisposable SetupOutputHandling()
	{
		if (trainer is not (OutputProvider outputProvider and OptionsHolder optionsHolder))
			return Disposable.Empty;
		var disposable = new CompositeDisposable();
		try
		{
			_timeEstimator = new RemainingTimeEstimator(optionsHolder.Options.Epochs);
			outputParser.Parse(outputProvider.Output);
			var epochResults = outputParser.Parse(outputProvider.Output).Publish();
			epochResults
				.Connect()
				.DisposeWith(disposable);
			epochResults
				.Select(ToProgress)
				.Subscribe(progressObserver)
				.DisposeWith(disposable);

			SetupArtifactsHandling(epochResults).DisposeWith(disposable);
			return disposable;
		}
		catch
		{
			disposable.Dispose();
			throw;
		}
	}

	private IDisposable SetupArtifactsHandling(IObservable<EpochResult> epochResults)
	{
		if (trainer is not TrainingPathsProvider pathsProvider)
			return Disposable.Empty;
		var artifactsProvider = new FileSystemWatcherTrainingArtifactsProvider(
			pathsProvider.OutputDirectoryPath,
			"*.pth",
			epochResults,
			logger.ForContext<FileSystemWatcherTrainingArtifactsProvider>());
		return artifactsProvider;

	}

	private Progress ToProgress(EpochResult epochResult)
	{
		Guard.IsNotNull(_timeEstimator);
		return new Progress
		{
			Label = "Training",
			Total = ((OptionsHolder)trainer).Options.Epochs,
			Current = epochResult.EpochNumber + 1,
			EstimatedTimeOfArrival = DateTime.Now + _timeEstimator.Estimate(epochResult.EpochNumber + 1)
		};
	}
}