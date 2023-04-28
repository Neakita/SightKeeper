namespace SightKeeper.Application.Input;

public interface HotKeyManager<in TKey>
{
	HotKey Register(TKey button, Action<HotKey>? pressed = null, Action<HotKey>? released = null);
}