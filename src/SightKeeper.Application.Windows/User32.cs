//https://www.cyberforum.ru/csharp-net/thread1385397.html

using System.Runtime.InteropServices;

namespace SightKeeper.Application.Windows;

/// <summary>
/// Helper class containing User32 API functions
/// </summary>
internal static class User32
{
	[DllImport("user32.dll")]
	internal static extern IntPtr GetForegroundWindow();

	[DllImport("user32.dll")]
	private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

	public static void MouseMove(int xDelta, int yDelta)
	{
		mouse_event(0x0001, xDelta, yDelta, 0, 0);
	}

	private const int WsExTransparent = 0x00000020;
	// ReSharper disable once IdentifierTypo
	private const int GwlExstyle = -20;
	[DllImport("user32.dll")]
	private static extern int GetWindowLong(IntPtr handle, int index);
	[DllImport("user32.dll")]
	private static extern int SetWindowLong(IntPtr handle, int index, int newStyle);
	
	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

	private const int WsExLayered = 0x80000;

	public static void MakeWindowClickThrough(IntPtr windowHandle)
	{
		var windowStyle = GetWindowLong(windowHandle, GwlExstyle);
		SetWindowLong(windowHandle, GwlExstyle, windowStyle | WsExLayered | WsExTransparent);
		SetLayeredWindowAttributes(windowHandle, 0, 255, 0x2);
	}
}