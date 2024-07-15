using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Profiles.Modules.Scaling;

public sealed class IterativeScalingOptions : PassiveScalingOptions
{
	public float MinimumScaling
	{
		get => _minimumScaling;
		set
		{
			Guard.IsGreaterThanOrEqualTo(value, 1);
			_minimumScaling = value;
		}
	}

	public float MaximumScaling
	{
		get => _maximumScaling;
		set
		{
			Guard.IsGreaterThan(value, 1);
			_maximumScaling = value;
		}
	}

	private float _minimumScaling = 1;
	private float _maximumScaling = 2;
}