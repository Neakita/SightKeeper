using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles.Behaviors;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Domain.Model.Profiles.Modules;

public abstract class ObjectiveModule : Module
{
	public ActiveScalingOptions? ActiveScalingOptions { get; set; }
	public ActiveWalkingOptions? ActiveWalkingOptions { get; set; }

	public Behavior Behavior { get; private set; }

	public T SetBehavior<T>() where T : Behavior, BehaviorFactory<T>
	{
		var behavior = T.CreateBehavior(this);
		Behavior = behavior;
		return behavior;
	}

	public ObjectiveModule(Profile profile) : base(profile)
	{
		Behavior = new AimBehavior(this);
	}
}

public abstract class ObjectiveModule<TWeights> : ObjectiveModule
	where TWeights : Weights
{
	public sealed override TWeights Weights => _weights;

	public void SetWeights(TWeights weights)
	{
		_weights = weights;
		Behavior.RemoveInappropriateTags();
	}

	protected ObjectiveModule(Profile profile, TWeights weights) : base(profile)
	{
		_weights = weights;
	}

	private TWeights _weights;
}