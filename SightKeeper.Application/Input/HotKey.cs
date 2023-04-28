namespace SightKeeper.Application.Input;

public interface HotKey : IDisposable
{
	bool IsPressed { get; }
}