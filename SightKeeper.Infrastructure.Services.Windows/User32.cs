﻿using System.Runtime.InteropServices;
using System.Text;
//https://www.cyberforum.ru/csharp-net/thread1385397.html
namespace SightKeeper.Infrastructure.Services.Windows;

/// <summary>
/// Helper class containing User32 API functions
/// </summary>
public static class User32
{
	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}
	[DllImport("user32.dll")]
	public static extern IntPtr GetDesktopWindow();
	[DllImport("user32.dll")]
	public static extern IntPtr GetWindowDC(IntPtr hWnd);
	[DllImport("user32.dll")]
	public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
	[DllImport("user32.dll")]
	public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
	
	[DllImport("user32.dll")]
	public static extern IntPtr GetForegroundWindow();
	
	[DllImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr GetDC(IntPtr hWnd);
	
	/// <summary>
	///    Performs a bit-block transfer of the color data corresponding to a
	///    rectangle of pixels from the specified source device context into
	///    a destination device context.
	/// </summary>
	/// <param name="hdc">Handle to the destination device context.</param>
	/// <param name="nXDest">The leftmost x-coordinate of the destination rectangle (in pixels).</param>
	/// <param name="nYDest">The topmost y-coordinate of the destination rectangle (in pixels).</param>
	/// <param name="nWidth">The width of the source and destination rectangles (in pixels).</param>
	/// <param name="nHeight">The height of the source and the destination rectangles (in pixels).</param>
	/// <param name="hdcSrc">Handle to the source device context.</param>
	/// <param name="nXSrc">The leftmost x-coordinate of the source rectangle (in pixels).</param>
	/// <param name="nYSrc">The topmost y-coordinate of the source rectangle (in pixels).</param>
	/// <param name="dwRop">A raster-operation code.</param>
	/// <returns>
	///    <c>true</c> if the operation succeedes, <c>false</c> otherwise. To get extended error information, call <see cref="System.Runtime.InteropServices.Marshal.GetLastWin32Error"/>.
	/// </returns>
	[DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

	public enum TernaryRasterOperations : uint
	{
		SRCCOPY = 0x00CC0020,
		SRCPAINT = 0x00EE0086,
		SRCAND = 0x008800C6,
		SRCINVERT = 0x00660046,
		SRCERASE = 0x00440328,
		NOTSRCCOPY = 0x00330008,
		NOTSRCERASE = 0x001100A6,
		MERGECOPY = 0x00C000CA,
		MERGEPAINT = 0x00BB0226,
		PATCOPY = 0x00F00021,
		PATPAINT = 0x00FB0A09,
		PATINVERT = 0x005A0049,
		DSTINVERT = 0x00550009,
		BLACKNESS = 0x00000042,
		WHITENESS = 0x00FF0062,
		CAPTUREBLT = 0x40000000
	}
}