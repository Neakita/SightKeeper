using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using CommunityToolkit.Diagnostics;
using Serilog;
using SightKeeper.Application.Training.RFDETR;
using Vibrance;

namespace SightKeeper.Application.Training;

internal sealed class FileSystemWatcherTrainingArtifactsProvider : TrainingArtifactsProvider, IDisposable
{
	public ReadOnlyObservableList<WeightsArtifact> Artifacts => _artifacts;

	public FileSystemWatcherTrainingArtifactsProvider(string path, string filter, IObservable<EpochResult> epochs, ILogger logger)
	{
		_logger = logger;
		_watcher = new FileSystemWatcher(path, filter);
		_watcher.Created += OnFileCreated;
		_watcher.Changed += OnFileChanged;
		_watcher.Deleted += OnFileDeleted;
		_watcher.Error += OnWatcherError;
		_watcher.EnableRaisingEvents = true;
		AddExistingArtifacts(path, filter);
		epochs.Subscribe(OnEpochPassed).DisposeWith(_disposable);
	}

	private void AddExistingArtifacts(string path, string filter)
	{
		var filePaths = Directory.GetFiles(path, filter);
		foreach (var filePath in filePaths)
		{
			_logger.Debug("Adding existing artifact \"{FilePath}\"", filePath);
			var fileName = Path.GetFileName(filePath);
			var artifact = new WeightsArtifact
			{
				FileName = fileName
			};
			_artifacts.Add(artifact);
		}
	}

	public void Dispose()
	{
		_watcher.Created -= OnFileCreated;
		_watcher.Changed -= OnFileChanged;
		_watcher.Deleted -= OnFileDeleted;
		_watcher.Error -= OnWatcherError;
		_artifacts.Dispose();
		_watcher.Dispose();
	}

	private readonly ObservableList<WeightsArtifact> _artifacts = new();
	private readonly FileSystemWatcher _watcher;
	private readonly ILogger _logger;
	private readonly CompositeDisposable _disposable = new();
	private readonly HashSet<string> _filesChangedLastEpoch = new();

	private void OnFileCreated(object sender, FileSystemEventArgs e)
	{
		Guard.IsNotNull(e.Name);
		_logger.Verbose("File \"{FileName}\" creation observed", e.Name);
		var artifact = new WeightsArtifact
		{
			FileName = e.Name
		};
		_artifacts.Add(artifact);
		_filesChangedLastEpoch.Add(e.Name);
	}

	private void OnFileChanged(object sender, FileSystemEventArgs e)
	{
		_logger.Verbose("File \"{FileName}\" change observed", e.Name);
		if (e.Name != null)
			_filesChangedLastEpoch.Add(e.Name);
	}

	private void OnFileDeleted(object sender, FileSystemEventArgs e)
	{
		_logger.Debug("File \"{FileName}\" deletion observed", e.Name);
		var index = _artifacts.Index().First(artifact => artifact.Item.FileName == e.Name).Index;
		_artifacts.RemoveAt(index);
	}

	private void OnWatcherError(object sender, ErrorEventArgs e)
	{
		_logger.Error(e.GetException(), $"An error has occured in {nameof(FileSystemWatcher)}");
	}

	private void OnEpochPassed(EpochResult result)
	{
		foreach (var artifact in _artifacts.Where(artifact => _filesChangedLastEpoch.Contains(artifact.FileName)))
		{
			artifact.EpochResult = result;
			_logger.Verbose("Artifact {fileName} epoch result updated", artifact.FileName);
		}
		_filesChangedLastEpoch.Clear();
	}
}