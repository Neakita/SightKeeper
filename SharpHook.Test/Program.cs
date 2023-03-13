using SharpHook;

TaskPoolGlobalHook hook = new();

hook.HookEnabled += OnHookEnabled;     // EventHandler<HookEventArgs>
hook.HookDisabled += OnHookDisabled;   // EventHandler<HookEventArgs>

hook.KeyTyped += OnKeyTyped;           // EventHandler<KeyboardHookEventArgs>
hook.KeyPressed += OnKeyPressed;       // EventHandler<KeyboardHookEventArgs>
hook.KeyReleased += OnKeyReleased;     // EventHandler<KeyboardHookEventArgs>

hook.MouseClicked += OnMouseClicked;   // EventHandler<MouseHookEventArgs>
hook.MousePressed += OnMousePressed;   // EventHandler<MouseHookEventArgs>
hook.MouseReleased += OnMouseReleased; // EventHandler<MouseHookEventArgs>
hook.MouseMoved += OnMouseMoved;       // EventHandler<MouseHookEventArgs>
hook.MouseDragged += OnMouseDragged;   // EventHandler<MouseHookEventArgs>

hook.MouseWheel += OnMouseWheel;       // EventHandler<MouseWheelHookEventArgs>

hook.Run();

Console.Read();

void OnHookEnabled(object? sender, HookEventArgs hookEventArgs)
{
	Console.WriteLine($"Hook Enabled: {hookEventArgs}");
}

void OnHookDisabled(object? sender, HookEventArgs hookEventArgs)
{
	Console.WriteLine($"Hook Disabled: {hookEventArgs}");
}

void OnKeyTyped(object? sender, KeyboardHookEventArgs keyboardHookEventArgs)
{
	Console.WriteLine($"Key Typed: {keyboardHookEventArgs.Data}");
}

void OnKeyPressed(object? sender, KeyboardHookEventArgs keyboardHookEventArgs)
{
	Console.WriteLine($"Key Pressed: {keyboardHookEventArgs.Data}");
}

void OnKeyReleased(object? sender, KeyboardHookEventArgs keyboardHookEventArgs)
{
	Console.WriteLine($"Key Released: {keyboardHookEventArgs.Data}");
}

void OnMouseClicked(object? sender, MouseHookEventArgs mouseHookEventArgs)
{
	Console.WriteLine($"Mouse Clicked: {mouseHookEventArgs.Data}");
}

void OnMousePressed(object? sender, MouseHookEventArgs mouseHookEventArgs)
{
	Console.WriteLine($"Mouse Pressed: {mouseHookEventArgs.Data}");
}

void OnMouseReleased(object? sender, MouseHookEventArgs mouseHookEventArgs)
{
	Console.WriteLine($"Mouse Released: {mouseHookEventArgs.Data}");
}

void OnMouseMoved(object? sender, MouseHookEventArgs mouseHookEventArgs)
{
	Console.WriteLine($"Mouse Moved: {mouseHookEventArgs.Data}");
}

void OnMouseDragged(object? sender, MouseHookEventArgs mouseHookEventArgs)
{
	Console.WriteLine($"Mouse Dragged: {mouseHookEventArgs.Data}");
}

void OnMouseWheel(object? sender, MouseWheelHookEventArgs mouseWheelHookEventArgs)
{
	Console.WriteLine($"Mouse Wheel: {mouseWheelHookEventArgs.Data}");
}