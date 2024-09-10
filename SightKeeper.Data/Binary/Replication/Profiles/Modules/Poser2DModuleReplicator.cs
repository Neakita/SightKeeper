using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Replication.Profiles.Modules;

internal sealed class Poser2DModuleReplicator : ObjectiveModuleReplicator
{
	protected override Poser2DModule CreateModule(Profile profile, Weights weights)
	{
		return profile.CreateModule((PoserWeights<Poser2DTag, KeyPointTag2D>)weights);
	}
}