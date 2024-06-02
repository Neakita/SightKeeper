using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels;

internal partial class GameViewModel : ViewModel
{
	public Game Game { get; }
	public string Title
	{
		get => Game.Title;
		set => SetProperty(Game.Title, value, title => Game.Title = title);
	}
	public string ProcessName
	{
		get => Game.ProcessName;
		set => SetProperty(Game.ProcessName, value, processName => Game.ProcessName = processName);
	}
	public string ExecutablePath
	{
		get => Game.ExecutablePath;
		set
		{
			if (SetProperty(Game.ExecutablePath, value, executablePath => Game.ExecutablePath = executablePath))
			{
				UpdateIcon();
				ShowExecutableCommand.NotifyCanExecuteChanged();
			}
		}
	}
	public Bitmap? Icon
	{
		get => _icon;
		private set => SetProperty(ref _icon, value);
	}

	public GameViewModel(Game game, GameIconProvider iconProvider, GameExecutableDisplayer executableDisplayer)
	{
		Game = game;
		_iconProvider = iconProvider;
		_executableDisplayer = executableDisplayer;
		_icon = GetIcon();
	}

	private readonly GameIconProvider _iconProvider;
	private readonly GameExecutableDisplayer _executableDisplayer;
	private Bitmap? _icon;

	private void UpdateIcon()
	{
		Icon = GetIcon();
	}

	private Bitmap? GetIcon()
	{
		var data = _iconProvider.GetIcon(Game);
		if (data == null)
			return null;
		using MemoryStream stream = new(data);
		return new Bitmap(stream);
	}

	private bool CanShowExecutable => !string.IsNullOrEmpty(ExecutablePath);

	[RelayCommand(CanExecute = nameof(CanShowExecutable))]
	private void ShowExecutable()
	{
		_executableDisplayer.ShowGameExecutable(Game);
	}
}