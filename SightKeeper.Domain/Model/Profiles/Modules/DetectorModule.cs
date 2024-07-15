using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.Profiles.Behaviours;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Domain.Model.Profiles.Modules;

public sealed class DetectorModule : Module
{
	public override DetectorWeights Weights => _weights;
	public AimBehaviour Behaviour { get; }

	public ActiveScalingOptions? ActiveScalingOptions { get; set; }
	public ActiveWalkingOptions? ActiveWalkingOptions { get; set; }

	public void SetWeights(DetectorWeights weights, ImmutableDictionary<Tag, AimBehaviour.TagOptions> tags)
	{
		_weights = weights;
		try
		{
			Behaviour.Tags = tags;
		}
		catch
		{
			Behaviour.Tags = ImmutableDictionary<Tag, AimBehaviour.TagOptions>.Empty;
			throw;
		}
	}

	internal DetectorModule(Profile profile, DetectorWeights weights) : base(profile)
	{
		_weights = weights;
		Behaviour = new AimBehaviour(this);
	}

	private DetectorWeights _weights;
}