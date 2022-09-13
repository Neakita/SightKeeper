using System.Diagnostics;
using System.Runtime.InteropServices;
using SightKeeper.Backend.Data.Members;

namespace SightKeeper.Backend;

public sealed class ForegroundGameMonitor
{
	public delegate void GameHandler(Game? game);
	public event GameHandler? ForegroundGameChanged;

	private Process? _currentProcess;


	public ForegroundGameMonitor(IEnumerable<Game> trackedGames)
	{
		TrackedGames = trackedGames;
		SetWinEventHook(EventSystemForeground, EventSystemForeground, IntPtr.Zero, WinEventProc, 0, 0,
			WinEventOutOfContext);
	}


	public IEnumerable<Game> TrackedGames
	{
		set => _trackedGames = value.ToDictionary(game => game.ProcessName);
	}

	
	private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
	private const uint WinEventOutOfContext = 0;
	private const uint EventSystemForeground = 3;
	private Dictionary<string, Game> _trackedGames = new();


	[DllImport("user32.dll")]
	private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr winEventProc, WinEventDelegate winEventProcDelegate, uint processId, uint threadId, uint flags);
	
	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();

	private static Process? GetForegroundProcess()
	{
		IntPtr windowPointer = GetForegroundWindow();
		return windowPointer == IntPtr.Zero ? null : Process.GetProcesses().First(process => process.MainWindowHandle == windowPointer);
	}
	
	private Game? GetForegroundGame() =>
		_currentProcess != null && _trackedGames.TryGetValue(_currentProcess.ProcessName, out Game? foregroundGame) ? foregroundGame : null;

	private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
	{
		_currentProcess = GetForegroundProcess();
		ForegroundGameChanged?.Invoke(GetForegroundGame());
	}
}
