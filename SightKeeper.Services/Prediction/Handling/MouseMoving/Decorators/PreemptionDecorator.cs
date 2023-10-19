using System.Numerics;
using Serilog;
using SightKeeper.Services.Prediction.Handling.MouseMoving.Decorators.Preemption;

namespace SightKeeper.Services.Prediction.Handling.MouseMoving.Decorators;

public sealed class PreemptionDecorator : DetectionMouseMover
{
	public PreemptionDecorator(DetectionMouseMover mouseMover, PreemptionComputer preemptionComputer)
	{
		_mouseMover = mouseMover;
		_preemptionComputer = preemptionComputer;
	}
	
	public void Move(MouseMovingContext context, Vector2 vector)
	{
		var now = DateTime.UtcNow;
		var timeDelta = now - _previousMoveTime;
		var preemption = Vector2.Zero;
		if (!_isFirstMove)
			preemption = _preemptionComputer.ComputePreemption(vector, timeDelta);
		else
			_isFirstMove = false;

		_mouseMover.Move(context, vector + preemption);
		_previousMoveTime = now;
	}

	public void OnPaused()
	{
		_isFirstMove = true;
		_preemptionComputer.Reset();
		Log.ForContext<PreemptionDecorator>().Debug("State cleared");
	}

	private readonly DetectionMouseMover _mouseMover;
	private readonly PreemptionComputer _preemptionComputer;
	private bool _isFirstMove = true;
	private DateTime _previousMoveTime;
}