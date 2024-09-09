using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles.Behaviors;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Domain.Model.Profiles.Modules;

public abstract class Module
{
	public Profile Profile { get; }
	public abstract Weights Weights { get; }
	public abstract Behavior Behavior { get; }

	public PassiveScalingOptions? PassiveScalingOptions
	{
		get => _passiveScalingOptions;
		set
		{
			Guard.IsFalse(value is IterativeScalingOptions && PassiveWalkingOptions is IterativeWalkingOptions);
			_passiveScalingOptions = value;
		}
	}

	public PassiveWalkingOptions? PassiveWalkingOptions
	{
		get => _passiveWalkingOptions;
		set
		{
			Guard.IsFalse(value is IterativeWalkingOptions && PassiveScalingOptions is IterativeScalingOptions);
			_passiveWalkingOptions = value;
		}
	}

	protected Module(Profile profile)
	{
		Profile = profile;
	}

	private PassiveScalingOptions? _passiveScalingOptions;
	private PassiveWalkingOptions? _passiveWalkingOptions;
}