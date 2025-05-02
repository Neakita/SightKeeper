namespace SightKeeper.Application.ScreenCapturing.Saving;

public interface LimitedSaver
{
	bool IsLimitReached { get; }
	Task Processing { get; }
}