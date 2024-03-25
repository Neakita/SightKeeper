using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels;

internal sealed class GameViewModel : ViewModel
{
	public string Title
	{
		get => _game.Title;
		set
		{
			SetProperty(_game.Title, value, title => _game.Title = title);
		}
	}
	public string ProcessName
	{
		get => _game.ProcessName;
		set
		{
			SetProperty(_game.ProcessName, value, processName => _game.ProcessName = processName);
		}
	}
	public string? ExecutablePath
	{
		get => _game.ExecutablePath;
		set
		{
			SetProperty(_game.ExecutablePath, value, executablePath => _game.ExecutablePath = executablePath);
		}
	}

	public GameViewModel(Game game)
	{
		_game = game;
	}

	private readonly Game _game;
}