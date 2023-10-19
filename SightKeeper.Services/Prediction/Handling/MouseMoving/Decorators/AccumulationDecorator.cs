using System.Numerics;
using Serilog;

namespace SightKeeper.Services.Prediction.Handling.MouseMoving.Decorators;

public sealed class AccumulationDecorator : DetectionMouseMover
{
	public Vector2 Accumulation { get; private set; } = Vector2.Zero;
	
	public AccumulationDecorator(DetectionMouseMover mouseMover)
	{
		_mouseMover = mouseMover;
	}
	
	public void Move(MouseMovingContext context, Vector2 vector)
	{
		var withAccumulation = vector + Accumulation;
		var rounded = RoundVector(withAccumulation);
		var difference = withAccumulation - rounded;
#if DEBUG
		Log.ForContext<AccumulationDecorator>().Debug(
			"Accumulation decorator received move vector {MoveVector}, " +
			"current accumulation is {Accumulation}, " +
			"move with added accumulation is {WithAccumulation}, " +
			"rounded to {Rounded}, " +
			"New accumulation is {Difference}",
			vector, Accumulation, withAccumulation, rounded, difference);
#endif
		Accumulation = difference;
		_mouseMover.Move(context, rounded);
	}
	
	private readonly DetectionMouseMover _mouseMover;

	private static Vector2 RoundVector(Vector2 vector) => new((float)Math.Round(vector.X), (float)Math.Round(vector.Y));
}