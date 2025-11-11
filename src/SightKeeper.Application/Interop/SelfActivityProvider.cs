namespace SightKeeper.Application.Interop;

public interface SelfActivityProvider
{
    bool IsOwnWindowActive { get; }
}