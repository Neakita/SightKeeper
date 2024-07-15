using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Profiles.Modules.Scaling;

public sealed class AdaptiveScalingOptions : ActiveScalingOptions
{
	public float Margin
	{
		get => _margin;
		set
		{
			Guard.IsGreaterThanOrEqualTo(value, 0);
			_margin = value;
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

	private float _margin;
	private float _maximumScaling = 2;
}