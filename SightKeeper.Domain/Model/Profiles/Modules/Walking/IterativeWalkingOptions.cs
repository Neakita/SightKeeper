﻿using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Profiles.Modules.Walking;

public sealed class IterativeWalkingOptions : PassiveWalkingOptions
{
	public Vector2<float> OffsetStep
	{
		get => _offsetStep;
		set
		{
			const float minimumValue = 0;
			Guard.IsGreaterThan(value.X, minimumValue);
			Guard.IsGreaterThan(value.Y, minimumValue);
			_offsetStep = value;
		}
	}

	public Vector2<byte> MaximumSteps
	{
		get => _maximumOffset;
		set
		{
			const byte minimumValue = 1;
			Guard.IsGreaterThanOrEqualTo(value.X, minimumValue);
			Guard.IsGreaterThanOrEqualTo(value.Y, minimumValue);
			_maximumOffset = value;
		}
	}

	private Vector2<float> _offsetStep = new(1, 1);
	private Vector2<byte> _maximumOffset = new(2, 2);
}