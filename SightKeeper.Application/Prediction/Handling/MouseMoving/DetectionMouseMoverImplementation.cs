using System.Numerics;

namespace SightKeeper.Application.Prediction.Handling.MouseMoving;

public sealed class DetectionMouseMoverImplementation : DetectionMouseMover
{
	public DetectionMouseMoverImplementation(MouseMover mouseMover)
	{
		_mouseMover = mouseMover;
	}

	public void Move(MouseMovingContext context, Vector2 vector)
	{
		_mouseMover.Move((int)vector.X, (int)vector.Y);
	}
	
	private readonly MouseMover _mouseMover;
}