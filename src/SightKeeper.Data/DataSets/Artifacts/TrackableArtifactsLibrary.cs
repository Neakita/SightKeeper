using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Artifacts;

namespace SightKeeper.Data.DataSets.Artifacts;

internal sealed class TrackableArtifactsLibrary(ArtifactsLibrary inner, ChangeListener listener) : ArtifactsLibrary, Decorator<ArtifactsLibrary>
{
	public IReadOnlyCollection<Artifact> Artifacts => inner.Artifacts;
	public ArtifactsLibrary Inner => inner;

	public Artifact CreateArtifact(ArtifactMetadata metadata)
	{
		var artifact = inner.CreateArtifact(metadata);
		listener.SetDataChanged();
		return artifact;
	}

	public void RemoveArtifact(Artifact artifact)
	{
		inner.RemoveArtifact(artifact);
		listener.SetDataChanged();
	}
}