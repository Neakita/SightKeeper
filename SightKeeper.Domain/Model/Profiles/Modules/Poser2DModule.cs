using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Model.Profiles.Modules;

public sealed class Poser2DModule : ObjectiveModule<Weights<Poser2DTag, KeyPointTag2D>>
{
	internal Poser2DModule(Profile profile, Weights<Poser2DTag, KeyPointTag2D> weights) : base(profile, weights)
	{
	}
}