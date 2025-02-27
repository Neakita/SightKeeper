namespace SightKeeper.Application.ScreenCapturing.Saving;

public interface LimitedSession
{
	Task Limit { get; }
}