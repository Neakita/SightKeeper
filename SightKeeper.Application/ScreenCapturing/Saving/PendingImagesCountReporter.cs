namespace SightKeeper.Application.ScreenCapturing.Saving;

public interface PendingImagesCountReporter
{
	BehaviorObservable<ushort> PendingImagesCount { get; }
}