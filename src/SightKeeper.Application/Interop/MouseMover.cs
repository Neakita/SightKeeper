namespace SightKeeper.Application.Interop;

public interface MouseMover
{
    IObservable<(int xDelta, int yDelta)> Moved { get; }
    void Move(int xDelta, int yDelta);
}