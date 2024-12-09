namespace SightKeeper.Domain.Screenshots;

internal abstract class ScreenshotsUsageProvider
{
	public abstract bool IsInUse(Screenshot screenshot);
}