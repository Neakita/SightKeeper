using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Artifacts;

namespace SightKeeper.Data.DataSets.Artifacts;

internal sealed class DataRemovingArtifactsLibrary(ArtifactsLibrary inner) : ArtifactsLibrary, Decorator<ArtifactsLibrary>
{
	public IReadOnlyCollection<Artifact> Artifacts => inner.Artifacts;
	public ArtifactsLibrary Inner => inner;

	public Artifact CreateArtifact(ArtifactMetadata metadata)
	{
		return inner.CreateArtifact(metadata);
	}

	public void RemoveArtifact(Artifact artifact)
	{
		inner.RemoveArtifact(artifact);
		artifact.GetFirst<DeletableData>().DeleteData();
	}
}