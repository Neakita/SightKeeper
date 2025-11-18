using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Artifacts;

namespace SightKeeper.Data.DataSets.Artifacts;

internal sealed class StorableArtifactWrapper : Wrapper<Artifact>
{
	public Artifact Wrap(Artifact artifact)
	{
		return artifact;
	}
}