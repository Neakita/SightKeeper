using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Model.Profiles.Modules;

public sealed class DetectorModule : ObjectiveModule<PlainWeights<DetectorTag>>
{
	internal DetectorModule(Profile profile, PlainWeights<DetectorTag> weights) : base(profile, weights)
	{
	}
}