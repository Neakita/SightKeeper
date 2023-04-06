using SharpHook.Native;

namespace SightKeeper.Application;

public interface KeyHook
{
	delegate void KeyHandler(KeyHook sender, KeyCode key);
	delegate void MouseButtonHandler(KeyHook sender, MouseButton button);

	event KeyHandler? KeyPressed;
	event KeyHandler? KeyReleased;
	event MouseButtonHandler? MouseButtonPressed;
	event MouseButtonHandler? MouseButtonReleased;

	bool IsPressed(KeyCode key);
	bool IsPressed(MouseButton button);
}
