using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Replication.Profiles.Modules;

internal sealed class DetectorModuleReplicator : ObjectiveModuleReplicator
{
	protected override DetectorModule CreateModule(Profile profile, Weights weights)
	{
		var typedWeights = (PlainWeights<DetectorTag>)weights;
		return profile.CreateModule(typedWeights);
	}
}