using System.Numerics;

namespace SightKeeper.Services.Prediction.Handling.MouseMoving;

public interface DetectionMouseMover
{
	void Move(MouseMovingContext context, Vector2 vector);
}