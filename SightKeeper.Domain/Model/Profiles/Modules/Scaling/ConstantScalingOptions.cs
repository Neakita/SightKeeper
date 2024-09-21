using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Profiles.Modules.Scaling;

public sealed class ConstantScalingOptions : PassiveScalingOptions
{
	public const float DefaultFactor = 2;

	public float Factor
	{
		get => _factor;
		set
		{
			Guard.IsGreaterThan(value, 1);
			_factor = value;
		}
	}

	private float _factor;

	public ConstantScalingOptions(float factor = DefaultFactor)
	{
		_factor = factor;
	}
}