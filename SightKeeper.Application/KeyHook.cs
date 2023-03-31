using SharpHook.Native;

namespace SightKeeper.Application;

public interface KeyHook
{
	delegate void KeyHandler(KeyCode key);

	event KeyHandler? KeyPressed;
	event KeyHandler? KeyReleased;
}
