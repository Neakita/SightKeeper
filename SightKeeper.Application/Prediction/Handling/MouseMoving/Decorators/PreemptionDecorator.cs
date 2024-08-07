using System.Numerics;
using Serilog;
using SightKeeper.Application.Prediction.Handling.MouseMoving.Decorators.Preemption;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Application.Prediction.Handling.MouseMoving.Decorators;

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
		if (_previousTag == context.TargetItem.Tag)
			preemption = _preemptionComputer.ComputePreemption(vector, timeDelta);
		else
			_preemptionComputer.Reset();
		_mouseMover.Move(context, vector + preemption);
		_previousMoveTime = now;
		_previousTag = context.TargetItem.Tag;
	}

	public void OnPaused()
	{
		_previousTag = null;
		_preemptionComputer.Reset();
		Log.ForContext<PreemptionDecorator>().Debug("State cleared");
	}

	private readonly DetectionMouseMover _mouseMover;
	private readonly PreemptionComputer _preemptionComputer;
	private DateTime _previousMoveTime;
	private Tag? _previousTag;
}