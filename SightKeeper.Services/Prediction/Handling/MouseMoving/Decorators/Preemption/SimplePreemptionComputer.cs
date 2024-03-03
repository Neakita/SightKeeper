using System.Numerics;
using CommunityToolkit.Diagnostics;
using Serilog;
using SightKeeper.Domain.Model;

namespace SightKeeper.Services.Prediction.Handling.MouseMoving.Decorators.Preemption;

public sealed class SimplePreemptionComputer : PreemptionComputer
{
	public SimplePreemptionComputer(Profile profile)
	{
		Guard.IsNotNull(profile.PreemptionSettings);
		_preemptionFactor = new Vector2(profile.PreemptionSettings.HorizontalFactor, profile.PreemptionSettings.VerticalFactor);
	}
	
	public Vector2 ComputePreemption(Vector2 moveVector, TimeSpan timeDelta)
	{
		var targetVelocity = moveVector + _previousPreemption;
		targetVelocity /= (float)timeDelta.TotalMilliseconds;
		var preemption = targetVelocity * BasePreemptionFactor * _preemptionFactor;
		Log.ForContext<SimplePreemptionComputer>().Debug("Preemption is {Preemption}", preemption);
		_previousPreemption = preemption;
		return preemption;
	}

	public void Reset()
	{
		_previousPreemption = Vector2.Zero;
	}

	private const float BasePreemptionFactor = 100;
	private readonly Vector2 _preemptionFactor;
	private Vector2 _previousPreemption = Vector2.Zero;
}