using System.Numerics;
using CommunityToolkit.Diagnostics;
using Serilog;
using SightKeeper.Domain.Model;

namespace SightKeeper.Services.Prediction.Handling.MouseMoving.Decorators;

public sealed class PreemptionDecorator : DetectionMouseMover
{
	public PreemptionDecorator(DetectionMouseMover mouseMover, Profile profile)
	{
		Guard.IsNotNull(profile.PreemptionSettings);
		_mouseMover = mouseMover;
		_preemptionFactor = new Vector2(profile.PreemptionSettings.HorizontalFactor, profile.PreemptionSettings.VerticalFactor);
	}
	
	public void Move(MouseMovingContext context, Vector2 vector)
	{
		var now = DateTime.UtcNow;
		var timeDelta = now - _previousMoveTime;
		var preemption = Vector2.Zero;
		if (!_isFirstMove)
			preemption = ComputePreemption(vector, timeDelta);
		else
			_isFirstMove = false;

		_mouseMover.Move(context, vector + preemption);
		_previousMoveTime = now;
	}

	private Vector2 ComputePreemption(Vector2 moveVector, TimeSpan timeDelta)
	{
		var targetVelocity = moveVector + _previousPreemption;
		targetVelocity /= (float)timeDelta.TotalMilliseconds;
		var preemption = targetVelocity * BasePreemptionFactor * _preemptionFactor;
		Log.ForContext<PreemptionDecorator>().Debug("Preemption is {Preemption}", preemption);
		_previousPreemption = preemption;
		return preemption;
	}

	public void OnPaused()
	{
		_isFirstMove = true;
		_previousPreemption = Vector2.Zero;
		Log.ForContext<PreemptionDecorator>().Debug("State cleared");
	}

	private const float BasePreemptionFactor = 100;
	private readonly DetectionMouseMover _mouseMover;
	private readonly Vector2 _preemptionFactor;
	private bool _isFirstMove = true;
	private DateTime _previousMoveTime;
	private Vector2 _previousPreemption = Vector2.Zero;
}