using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Services;

public sealed class DbGamesDataAccess : GamesDataAccess
{
	public IObservable<Game> GameAdded => _gameAdded;
	public IObservable<Game> GameRemoved => _gameRemoved;
	public IReadOnlyCollection<Game> Games => _games;

    public DbGamesDataAccess(AppDbContext dbContext)
    {
	    _games = new HashSet<Game>(dbContext.Games);
	    _dbContext = dbContext;
    }

    public void AddGame(Game game)
    {
	    bool isAdded = _games.Add(game);
	    Guard.IsTrue(isAdded);
        _dbContext.Games.Add(game);
        _gameAdded.OnNext(game);
    }

    public void RemoveGame(Game game)
    {
	    bool isRemoved = _games.Remove(game);
	    Guard.IsTrue(isRemoved);
        _dbContext.Remove(game);
        _gameRemoved.OnNext(game);
    }

    private readonly HashSet<Game> _games;
    private readonly AppDbContext _dbContext;
    private readonly Subject<Game> _gameAdded = new();
    private readonly Subject<Game> _gameRemoved = new();
}