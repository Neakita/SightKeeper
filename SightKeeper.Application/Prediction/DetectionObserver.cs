using Serilog;

namespace SightKeeper.Application.Prediction;

public interface DetectionObserver : IObserver<DetectionData>
{
	IObservable<float?> RequestedProbabilityThreshold { get; }

	void IObserver<DetectionData>.OnNext(DetectionData value)
	{
		Log.ForContext(GetType()).Information("Value observed");
	}

	void OnPaused()
	{
		Log.ForContext(GetType()).Information("Pause observed");
	}

	void IObserver<DetectionData>.OnCompleted()
	{
		Log.ForContext(GetType()).Information("Completion observed");
	}

	void IObserver<DetectionData>.OnError(Exception exception)
	{
		Log.ForContext(GetType()).Error(exception, "Error observed");
	}
}