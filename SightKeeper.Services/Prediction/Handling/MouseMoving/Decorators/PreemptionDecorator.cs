using System.Numerics;
using Serilog;
using SightKeeper.Domain.Model.Common;
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
		if (_previousItemClass == context.TargetItem.ItemClass)
			preemption = _preemptionComputer.ComputePreemption(vector, timeDelta);
		else
			_preemptionComputer.Reset();
		_mouseMover.Move(context, vector + preemption);
		_previousMoveTime = now;
		_previousItemClass = context.TargetItem.ItemClass;
	}

	public void OnPaused()
	{
		_previousItemClass = null;
		_preemptionComputer.Reset();
		Log.ForContext<PreemptionDecorator>().Debug("State cleared");
	}

	private readonly DetectionMouseMover _mouseMover;
	private readonly PreemptionComputer _preemptionComputer;
	private DateTime _previousMoveTime;
	private ItemClass? _previousItemClass;
}