using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Serilog;
using SightKeeper.Backend.Data.Members;

namespace SightKeeper.Backend;

public sealed class ForegroundGameMonitor : IDisposable
{
	public delegate void GameHandler(Game? game);
	public event GameHandler? ForegroundGameChanged;

	private Process? _currentProcess;

	// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
	private readonly WinEventDelegate _eventDelegate;


	public ForegroundGameMonitor(IEnumerable<Game> trackedGames)
	{
		TrackedGames = trackedGames;
		_eventDelegate = WinEventProc;
		_hook = SetWinEventHook(EventSystemForeground, EventSystemForeground, IntPtr.Zero, _eventDelegate, 0, 0,
			WinEventOutOfContext);
		if (_hook == IntPtr.Zero) throw new Win32Exception("Failed to subscribe to system event");
	}


	public IEnumerable<Game> TrackedGames
	{
		set => _trackedGames = value.ToDictionary(game => game.ProcessName);
	}

	
	private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
	private const uint WinEventOutOfContext = 0;
	private const uint EventSystemForeground = 3;
	private Dictionary<string, Game> _trackedGames = new();
	private readonly IntPtr _hook;


	[DllImport("user32.dll")]
	private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr winEventProc, WinEventDelegate winEventProcDelegate, uint processId, uint threadId, uint flags);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool UnhookWindowsHookEx(IntPtr hhk);
	
	
	[DllImport("user32.dll", SetLastError = true)]
	private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

	private static Process? GetForegroundProcess(IntPtr windowHandle)
	{
		GetWindowThreadProcessId(windowHandle, out uint processId);
		var foregroundProcess = Process.GetProcessById((int) processId);
		Log.Logger.Verbose("Current foreground process switched; process name: {ForegroundProcessName}, main window title: {ForegroundProcessMainWindowTitle}",
			foregroundProcess.ProcessName, foregroundProcess.MainWindowTitle);
		return foregroundProcess;
	}
	
	private Game? GetForegroundGame()
	{
		if (_currentProcess == null) return null;
		
		_trackedGames.TryGetValue(_currentProcess.ProcessName, out Game? foregroundGame);
		if (_currentProcess == null) return foregroundGame;
		
		if (foregroundGame == null) Log.Logger.Verbose("Current foreground game is not detected");
		else Log.Logger.Verbose("Current foreground game title: {GameTitle}", foregroundGame.Title);
		
		return foregroundGame;
	}

	private void WinEventProc(IntPtr eventHook, uint eventType, IntPtr windowHandle, int objectId, int childId, uint eventThread, uint eventTime)
	{
		_currentProcess = GetForegroundProcess(windowHandle);
		Game? currentGame = GetForegroundGame();
		ForegroundGameChanged?.Invoke(currentGame);
	}

	~ForegroundGameMonitor()
	{
		Dispose(false);
	}

	private void ReleaseUnmanagedResources()
	{
		UnhookWindowsHookEx(_hook);
	}

	private void Dispose(bool disposing)
	{
		ReleaseUnmanagedResources();
		if (disposing)
		{
			_currentProcess?.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
