using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.Profiles.Behaviours;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Domain.Model.Profiles.Modules;

public sealed class PoserModule : Module, Behavioural
{
	public override Weights<Poser2DTag, KeyPointTag2D> Weights => _weights;
	public Behaviour Behaviour { get; private set; }

	public ActiveScalingOptions? ActiveScalingOptions { get; set; }
	public ActiveWalkingOptions? ActiveWalkingOptions { get; set; }

	public void SetWeights(Weights<Poser2DTag, KeyPointTag2D> weights)
	{
		_weights = weights;
		Behaviour.RemoveInappropriateTags();
	}

	public AimBehaviour SetAimBehaviour()
	{
		AimBehaviour behaviour = new(this);
		Behaviour = behaviour;
		return behaviour;
	}

	public AimAssistBehaviour SetAimAssistBehaviour()
	{
		AimAssistBehaviour behaviour = new(this);
		Behaviour = behaviour;
		return behaviour;
	}

	public TriggerBehaviour SetTriggerBehaviour()
	{
		TriggerBehaviour behaviour = new(this);
		Behaviour = behaviour;
		return behaviour;
	}

	internal PoserModule(Profile profile, Weights<Poser2DTag, KeyPointTag2D> weights) : base(profile)
	{
		_weights = weights;
		Behaviour = new AimBehaviour(this);
	}

	private Weights<Poser2DTag, KeyPointTag2D> _weights;
}