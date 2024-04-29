using System.Diagnostics;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

internal readonly struct ProcessDescription
{
	public static ProcessDescription Extract(Process process)
	{
		return new ProcessDescription(process.MainWindowTitle, process.ProcessName, process.MainModule?.FileName);
	}

	public static ProcessDescription Extract(Game game) =>
		new(game.Title, game.ProcessName, game.ExecutablePath);

	public static bool operator ==(ProcessDescription left, ProcessDescription right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(ProcessDescription left, ProcessDescription right)
	{
		return !left.Equals(right);
	}

	public readonly string Title;
	public readonly string ProcessName;
	public readonly string? ExecutablePath;

	public bool Equals(ProcessDescription other)
	{
		return Title == other.Title && ProcessName == other.ProcessName && ExecutablePath == other.ExecutablePath;
	}

	public override bool Equals(object? obj)
	{
		return obj is ProcessDescription other && Equals(other);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Title, ProcessName, ExecutablePath);
	}

	private ProcessDescription(string title, string processName, string? executablePath)
	{
		Title = title;
		ProcessName = processName;
		ExecutablePath = executablePath;
	}
}