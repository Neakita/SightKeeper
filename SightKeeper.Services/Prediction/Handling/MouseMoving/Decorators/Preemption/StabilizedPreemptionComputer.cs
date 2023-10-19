using System.Numerics;
using CommunityToolkit.Diagnostics;
using Serilog;
using SightKeeper.Commons;
using SightKeeper.Domain.Model;

namespace SightKeeper.Services.Prediction.Handling.MouseMoving.Decorators.Preemption;

public sealed class StabilizedPreemptionComputer : PreemptionComputer
{
	public StabilizedPreemptionComputer(Profile profile)
	{
		Guard.IsNotNull(profile.PreemptionSettings?.StabilizationSettings);
		_preemptionFactor = new Vector2(profile.PreemptionSettings.HorizontalFactor, profile.PreemptionSettings.VerticalFactor);
		_velocities = new List<Vector2>(profile.PreemptionSettings.StabilizationSettings.BufferSize);
		_method = profile.PreemptionSettings.StabilizationSettings.Method switch
		{
			PreemptionStabilizationMethod.Median => EnumerableExtensions.Median,
			PreemptionStabilizationMethod.Mean => Enumerable.Average,
			_ => ThrowHelper.ThrowArgumentOutOfRangeException<Func<IEnumerable<float>, float>>()
		};
	}
	
	public Vector2 ComputePreemption(Vector2 moveVector, TimeSpan timeDelta)
	{
		var targetVelocity = moveVector + _previousPreemption;
		targetVelocity /= (float)timeDelta.TotalMilliseconds;
		
		if (_velocities.Count == _velocities.Capacity)
			_velocities.RemoveAt(0);
		_velocities.Add(targetVelocity);

		Vector2 preemption = new(
			_method(_velocities.Select(velocity => velocity.X)),
			_method(_velocities.Select(velocity => velocity.Y)));
		
		preemption *= BasePreemptionFactor * _preemptionFactor;
		Log.ForContext<StabilizedPreemptionComputer>().Debug("Preemption is {Preemption}", preemption);
		_previousPreemption = preemption;
		return preemption;
	}

	public void Reset()
	{
		_previousPreemption = Vector2.Zero;
	}
	
	private const float BasePreemptionFactor = 100;
	private readonly Vector2 _preemptionFactor;
	private readonly List<Vector2> _velocities;
	private readonly Func<IEnumerable<float>, float> _method;
	private Vector2 _previousPreemption = Vector2.Zero;
}