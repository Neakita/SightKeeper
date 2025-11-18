using FlakeId;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Artifacts;

namespace SightKeeper.Data.DataSets.Artifacts;

internal sealed class InMemoryArtifact(Id id, ArtifactMetadata metadata) : Artifact, IdHolder
{
	public Id Id => id;
	public ArtifactMetadata Metadata => metadata;
}