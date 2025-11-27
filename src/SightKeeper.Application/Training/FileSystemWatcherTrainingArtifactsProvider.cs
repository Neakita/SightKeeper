using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using CommunityToolkit.Diagnostics;
using Serilog;
using Vibrance;

namespace SightKeeper.Application.Training;

internal sealed class FileSystemWatcherTrainingArtifactsProvider : TrainingArtifactsProvider, IDisposable
{
	public ReadOnlyObservableList<TrainingArtifact> Artifacts => _artifacts;

	public FileSystemWatcherTrainingArtifactsProvider(string directoryPath, string filter, Func<string, TrainingArtifact> artifactFactory, IObservable<EpochResult> epochs, ILogger logger)
	{
		_artifactFactory = artifactFactory;
		_logger = logger;
		_watcher = new FileSystemWatcher(directoryPath, filter);
		_watcher.Created += OnFileCreated;
		_watcher.Changed += OnFileChanged;
		_watcher.Deleted += OnFileDeleted;
		_watcher.Error += OnWatcherError;
		_watcher.EnableRaisingEvents = true;
		epochs.Subscribe(OnEpochPassed).DisposeWith(_disposable);
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

	private readonly ObservableList<TrainingArtifact> _artifacts = new();
	private readonly FileSystemWatcher _watcher;
	private readonly Func<string, TrainingArtifact> _artifactFactory;
	private readonly ILogger _logger;
	private readonly CompositeDisposable _disposable = new();
	private readonly HashSet<TrainingArtifact> _artifactsChangedLastEpoch = new();

	private void OnFileCreated(object sender, FileSystemEventArgs e)
	{
		Guard.IsNotNull(e.Name);
		_logger.Verbose("File \"{FileName}\" creation observed", e.Name);
		var artifact = CreateArtifact(e.FullPath);
		_artifacts.Add(artifact);
		_artifactsChangedLastEpoch.Add(artifact);
	}

	private void OnFileChanged(object sender, FileSystemEventArgs e)
	{
		_logger.Verbose("File \"{FileName}\" change observed", e.Name);
		if (e.Name == null)
			return;
		var artifact = _artifacts.SingleOrDefault(artifact => artifact.FileName == e.Name);
		if (artifact == null)
		{
			artifact = CreateArtifact(e.FullPath);
			_artifacts.Add(artifact);
		}
		_artifactsChangedLastEpoch.Add(artifact);
		artifact.Timestamp = DateTime.Now;
	}

	private void OnFileDeleted(object sender, FileSystemEventArgs e)
	{
		_logger.Debug("File \"{FileName}\" deletion observed", e.Name);
		var index = _artifacts.Index().First(artifact => artifact.Item.FileName == e.Name).Index;
		var artifact = _artifacts[index];
		_artifacts.RemoveAt(index);
		_artifactsChangedLastEpoch.Remove(artifact);
	}

	private void OnWatcherError(object sender, ErrorEventArgs e)
	{
		_logger.Error(e.GetException(), $"An error has occured in {nameof(FileSystemWatcher)}");
	}

	private void OnEpochPassed(EpochResult result)
	{
		foreach (var artifact in _artifactsChangedLastEpoch)
		{
			artifact.Epoch = result;
			_logger.Verbose("Artifact {fileName} epoch result updated", artifact.FileName);
		}
		_artifactsChangedLastEpoch.Clear();
	}

	private TrainingArtifact CreateArtifact(string filePath)
	{
		return _artifactFactory(filePath);
	}
}