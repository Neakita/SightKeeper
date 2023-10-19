using System.Numerics;
using Serilog;

namespace SightKeeper.Services.Prediction.Handling.MouseMoving;

public interface DetectionMouseMover
{
	void Move(MouseMovingContext context, Vector2 vector);
	void OnPaused()
	{
		Log.ForContext(GetType()).Debug("Pause observed");
	}
}