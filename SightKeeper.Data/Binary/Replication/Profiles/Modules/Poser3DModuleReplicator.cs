using SightKeeper.Domain.Model.DataSets.Poser3D;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Replication.Profiles.Modules;

internal sealed class Poser3DModuleReplicator : ObjectiveModuleReplicator
{
	protected override Poser3DModule CreateModule(Profile profile, Weights weights)
	{
		return profile.CreateModule((PoserWeights<Poser3DTag, KeyPointTag3D>)weights);
	}
}