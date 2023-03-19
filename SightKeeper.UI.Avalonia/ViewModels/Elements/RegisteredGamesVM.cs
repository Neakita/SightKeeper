using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using ReactiveUI;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Infrastructure.Data;

namespace SightKeeper.UI.Avalonia.ViewModels.Elements;

public sealed class RegisteredGamesVM : ViewModel
{
	private readonly AppDbContextFactory _dbContextFactory;
	public ObservableCollection<Game> RegisteredGames { get; }

	public IReadOnlyCollection<Game> AvailableToAddGames => 
		Process.GetProcesses()
			.Where(process => process.MainWindowHandle != 0 && RegisteredGames.All(game => game.ProcessName != process.ProcessName))
			.Select(process => new Game(process.MainWindowTitle, process.ProcessName))
			.ToList();


	public RegisteredGamesVM(AppDbContextFactory dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
		using AppDbContext dbContext = dbContextFactory.CreateDbContext();
		RegisteredGames = new ObservableCollection<Game>(dbContext.Games);
		RegisteredGames.CollectionChanged += RegisteredGamesOnCollectionChanged;
	}

	private void RegisteredGamesOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		this.RaisePropertyChanged(nameof(RegisteredGames));
	}

	public void AddGame(Game game)
	{
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Games.Add(game);
		dbContext.SaveChanges();
		RegisteredGames.Add(game);
		RefreshAvailableToAddGames();
	}

	private bool CanAddGame(object? parameter) => parameter != null;

	public void DeleteGame(Game game)
	{
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Games.Remove(game);
		dbContext.SaveChanges();
		RegisteredGames.Remove(game);
		RefreshAvailableToAddGames();
	}

	private bool CanDeleteGame(object? parameter) => parameter != null;

	public void RefreshAvailableToAddGames() => this.RaisePropertyChanged(nameof(AvailableToAddGames));
}