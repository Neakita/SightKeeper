namespace SightKeeper.Application.Screenshotting.Saving;

public interface LimitedSession
{
	Task Limit { get; }
}