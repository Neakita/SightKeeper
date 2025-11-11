using System.Diagnostics;

namespace SightKeeper.Application.Misc;

public sealed class RemainingTimeEstimator(double finalProgress)
{
	public TimeSpan? Estimate(double progress)
	{
		if (progress == 0)
			return null;
		var elapsedTime = _stopwatch.Elapsed;
		var remainingProgress = finalProgress - progress;
		var remainingTime = elapsedTime / progress * remainingProgress;
		return remainingTime;
	}

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
}