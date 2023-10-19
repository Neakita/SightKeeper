using System.Numerics;

namespace SightKeeper.Services.Prediction.Handling.MouseMoving.Decorators.Preemption;

public interface PreemptionComputer
{
	Vector2 ComputePreemption(Vector2 moveVector, TimeSpan timeDelta);
	void Reset();
}