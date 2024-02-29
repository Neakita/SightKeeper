//https://www.cyberforum.ru/csharp-net/thread1385397.html

using System.Runtime.InteropServices;

namespace SightKeeper.Services.Windows;

/// <summary>
/// Helper class containing User32 API functions
/// </summary>
public static class User32
{
	[DllImport("user32.dll")]
	internal static extern IntPtr GetForegroundWindow();

	[DllImport("user32.dll")]
	private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

	public static void MouseMove(int xDelta, int yDelta)
	{
		mouse_event(0x0001, xDelta, yDelta, 0, 0);
	}

	private const int WS_EX_TRANSPARENT = 0x00000020;
	// ReSharper disable once IdentifierTypo
	private const int GWL_EXSTYLE = -20;
	[DllImport("user32.dll")]
	private static extern int GetWindowLong(IntPtr handle, int index);
	[DllImport("user32.dll")]
	private static extern int SetWindowLong(IntPtr handle, int index, int newStyle);
	
	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

	private const int WS_EX_LAYERED = 0x80000;

	public static void MakeWindowClickThrough(IntPtr windowHandle)
	{
		var windowStyle = GetWindowLong(windowHandle, GWL_EXSTYLE);
		SetWindowLong(windowHandle, GWL_EXSTYLE, windowStyle | WS_EX_LAYERED | WS_EX_TRANSPARENT);
		SetLayeredWindowAttributes(windowHandle, 0, 255, 0x2);
	}
}