using System.Numerics;
using Serilog;

namespace SightKeeper.Services.Prediction.Handling.MouseMoving.Decorators;

public sealed class PreemptionDecorator : DetectionMouseMover
{
	public PreemptionDecorator(DetectionMouseMover mouseMover)
	{
		_mouseMover = mouseMover;
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
		var preemption = targetVelocity * BasePreemptionFactor;
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
	private bool _isFirstMove = true;
	private DateTime _previousMoveTime;
	private Vector2 _previousPreemption = Vector2.Zero;
}