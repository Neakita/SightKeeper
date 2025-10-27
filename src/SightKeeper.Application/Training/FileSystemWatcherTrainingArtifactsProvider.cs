using CommunityToolkit.Diagnostics;
using Serilog;
using Vibrance;

namespace SightKeeper.Application.Training;

internal sealed class FileSystemWatcherTrainingArtifactsProvider : TrainingArtifactsProvider, IDisposable
{
	public ReadOnlyObservableList<WeightsArtifact> Artifacts => _artifacts;

	public FileSystemWatcherTrainingArtifactsProvider(string path, string filter, ILogger logger)
	{
		_logger = logger;
		_watcher = new FileSystemWatcher(path, filter);
		_watcher.Created += OnFileCreated;
		_watcher.Deleted += OnFileDeleted;
		_watcher.Error += OnWatcherError;
	}

	public void Dispose()
	{
		_watcher.Created -= OnFileCreated;
		_watcher.Deleted -= OnFileDeleted;
		_watcher.Error -= OnWatcherError;
		_artifacts.Dispose();
		_watcher.Dispose();
	}

	private readonly ObservableList<WeightsArtifact> _artifacts = new();
	private readonly FileSystemWatcher _watcher;
	private readonly ILogger _logger;

	private void OnFileCreated(object sender, FileSystemEventArgs e)
	{
		Guard.IsNotNull(e.Name);
		var artifact = new WeightsArtifact
		{
			FileName = e.Name
		};
		_artifacts.Add(artifact);
	}

	private void OnFileDeleted(object sender, FileSystemEventArgs e)
	{
		var index = _artifacts.Index().First(artifact => artifact.Item.FileName == e.Name).Index;
		_artifacts.RemoveAt(index);
	}

	private void OnWatcherError(object sender, ErrorEventArgs e)
	{
		_logger.Error(e.GetException(), $"An error has occured in {nameof(FileSystemWatcher)}");
	}
}