using SightKeeper.Domain.Model.DataSets.Poser3D;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Model.Profiles.Modules;

public sealed class Poser3DModule : ObjectiveModule<PoserWeights<Poser3DTag, KeyPointTag3D>>
{
	public Poser3DModule(Profile profile, PoserWeights<Poser3DTag, KeyPointTag3D> weights) : base(profile, weights)
	{
	}
}