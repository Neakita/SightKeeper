namespace SightKeeper.Application.Screenshotting.Saving;

public interface PendingScreenshotsCountReporter
{
	BehaviorObservable<ushort> PendingScreenshotsCount { get; }
}