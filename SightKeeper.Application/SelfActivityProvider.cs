namespace SightKeeper.Application;

public interface SelfActivityProvider
{
    bool IsOwnWindowActive { get; }
}