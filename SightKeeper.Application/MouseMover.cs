namespace SightKeeper.Application;

public interface MouseMover
{
    IObservable<(short xDelta, short yDelta)> Moved { get; }
    void Move(short xDelta, short yDelta);
    void SetPosition(short x, short y);
}