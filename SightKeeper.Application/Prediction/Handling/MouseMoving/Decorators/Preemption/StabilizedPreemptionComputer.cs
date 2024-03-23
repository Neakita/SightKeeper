using System.Numerics;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Application.Prediction.Handling.MouseMoving.Decorators.Preemption;

public sealed class StabilizedPreemptionComputer : PreemptionComputer
{
	public StabilizedPreemptionComputer(Profile profile)
	{
		Guard.IsNotNull(profile.PreemptionSettings?.StabilizationSettings);
		_preemptionFactor = new Vector2(profile.PreemptionSettings.Factor.X, profile.PreemptionSettings.Factor.Y);
		_velocities = new List<Vector2>(profile.PreemptionSettings.StabilizationSettings.BufferSize);
		_method = profile.PreemptionSettings.StabilizationSettings.Method switch
		{
			StabilizationMethod.Median => EnumerableExtensions.Median,
			StabilizationMethod.Mean => Enumerable.Average,
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
		_previousPreemption = preemption;
		return preemption;
	}

	public void Reset()
	{
		_previousPreemption = Vector2.Zero;
		_velocities.Clear();
	}
	
	private const float BasePreemptionFactor = 100;
	private readonly Vector2 _preemptionFactor;
	private readonly List<Vector2> _velocities;
	private readonly Func<IEnumerable<float>, float> _method;
	private Vector2 _previousPreemption = Vector2.Zero;
}