namespace SightKeeper.Application;

public interface MouseMover
{
    IObservable<(int xDelta, int yDelta)> Moved { get; }
    void Move(int xDelta, int yDelta);
    void SetPosition(int x, int y);
}