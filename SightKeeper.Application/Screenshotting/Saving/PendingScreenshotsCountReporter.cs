namespace SightKeeper.Application.Screenshotting.Saving;

public interface PendingScreenshotsCountReporter
{
	IObservable<ushort> PendingScreenshotsCount { get; }
}