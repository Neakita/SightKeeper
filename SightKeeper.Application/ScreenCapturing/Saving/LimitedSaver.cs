namespace SightKeeper.Application.ScreenCapturing.Saving;

public interface LimitedSaver
{
	Task Limit { get; }
}