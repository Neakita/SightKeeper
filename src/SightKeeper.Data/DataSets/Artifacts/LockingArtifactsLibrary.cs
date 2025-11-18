using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Artifacts;

namespace SightKeeper.Data.DataSets.Artifacts;

internal sealed class LockingArtifactsLibrary(ArtifactsLibrary inner, Lock editingLock) : ArtifactsLibrary, Decorator<ArtifactsLibrary>
{
	public IReadOnlyCollection<Artifact> Artifacts => inner.Artifacts;
	public ArtifactsLibrary Inner => inner;

	public Artifact CreateArtifact(ArtifactMetadata metadata)
	{
		lock (editingLock)
			return inner.CreateArtifact(metadata);
	}

	public void RemoveArtifact(Artifact artifact)
	{
		lock (editingLock)
			inner.RemoveArtifact(artifact);
	}
}