namespace SightKeeper.Domain.DataSets.Artifacts;

public interface ArtifactsLibrary
{
	IReadOnlyCollection<Artifact> Artifacts { get; }
	Artifact CreateArtifact(ArtifactMetadata metadata);
	void RemoveArtifact(Artifact artifact);
}