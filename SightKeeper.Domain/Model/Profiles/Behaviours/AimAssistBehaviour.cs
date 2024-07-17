﻿using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Domain.Model.Profiles.Behaviours;

public sealed class AimAssistBehaviour
{
	public record TagOptions(byte Priority, Vector2<float> TargetAreaScale, float VerticalOffset);

	public Module Module { get; }

	public ImmutableDictionary<Tag, TagOptions> Tags
	{
		get => _tags;
		set
		{
			foreach (var tag in value.Keys)
				Guard.IsTrue(Module.Weights.Contains(tag));
			_tags = value;
		}
	}
	
	public float DirectionCorrectionFactor
	{
		get => _directionCorrectionFactor;
		set
		{
			Guard.IsBetweenOrEqualTo(value, 0, 1);
			_directionCorrectionFactor = value;
		}
	}

	public float GainFactor
	{
		get => _gainFactor;
		set
		{
			Guard.IsGreaterThanOrEqualTo(value, 0);
			_gainFactor = value;
		}
	}

	public float AttenuationFactor
	{
		get => _attenuationFactor;
		set
		{
			Guard.IsBetweenOrEqualTo(value, 0, 1);
			_attenuationFactor = value;
		}
	}

	public AimAssistBehaviour(Module module)
	{
		Module = module;
	}

	private ImmutableDictionary<Tag, TagOptions> _tags = ImmutableDictionary<Tag, TagOptions>.Empty;
	private float _directionCorrectionFactor;
	private float _gainFactor;
	private float _attenuationFactor;
}