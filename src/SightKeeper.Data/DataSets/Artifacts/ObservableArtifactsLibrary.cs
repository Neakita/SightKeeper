using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Artifacts;
using Vibrance.Changes;

namespace SightKeeper.Data.DataSets.Artifacts;

internal sealed class ObservableArtifactsLibrary(ArtifactsLibrary inner) : ArtifactsLibrary, Decorator<ArtifactsLibrary>, IDisposable
{
	public IReadOnlyCollection<Artifact> Artifacts => _artifacts;
	public ArtifactsLibrary Inner => inner;

	public Artifact CreateArtifact(ArtifactMetadata metadata)
	{
		var artifacts = inner.CreateArtifact(metadata);
		var change = new Addition<Artifact>
		{
			Items = [artifacts]
		};
		_artifacts.Notify(change);
		return artifacts;
	}

	public void RemoveArtifact(Artifact artifact)
	{
		inner.RemoveArtifact(artifact);
		var change = new Removal<Artifact>
		{
			Items = [artifact]
		};
		_artifacts.Notify(change);
	}

	public void Dispose()
	{
		_artifacts.Dispose();
	}

	private readonly ExternalObservableCollection<Artifact> _artifacts = new(inner.Artifacts);
}