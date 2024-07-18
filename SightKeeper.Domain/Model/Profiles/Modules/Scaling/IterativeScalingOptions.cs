using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Profiles.Modules.Scaling;

public sealed class IterativeScalingOptions : PassiveScalingOptions
{
	public float Initial
	{
		get => _initial;
		set
		{
			Guard.IsGreaterThanOrEqualTo(value, 1);
			_initial = value;
		}
	}

	public float StepSize
	{
		get => _stepSize;
		set
		{
			Guard.IsGreaterThan(value, 0);
			_stepSize = value;
		}
	}

	public byte StepsCount
	{
		get => _stepsCount;
		set
		{
			const byte minimumValue = 2;
			Guard.IsGreaterThanOrEqualTo(value, minimumValue);
			_stepsCount = value;
		}
	}

	public IterativeScalingOptions(float initial = 1, float stepSize = 1, byte stepsCount = 2)
	{
		Initial = initial;
		StepSize = stepSize;
		StepsCount = stepsCount;
	}

	private float _initial;
	private float _stepSize;
	private byte _stepsCount;
}