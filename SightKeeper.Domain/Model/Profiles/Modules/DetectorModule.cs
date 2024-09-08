using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Model.Profiles.Modules;

public sealed class DetectorModule : ObjectiveModule<Weights<DetectorTag>>
{
	internal DetectorModule(Profile profile, Weights<DetectorTag> weights) : base(profile, weights)
	{
	}
}