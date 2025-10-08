namespace SightKeeper.Application.ScreenCapturing.Saving;

internal interface LimitedSaver
{
	bool IsLimitReached { get; }
	Task Processing { get; }
}