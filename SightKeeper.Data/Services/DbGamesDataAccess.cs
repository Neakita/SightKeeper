using CommunityToolkit.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbGamesDataAccess : GamesDataAccess
{
    public DbGamesDataAccess(AppDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<IReadOnlyCollection<Game>> GetGames(CancellationToken cancellationToken = default) =>
        await _dbContext.Games.ToListAsync(cancellationToken);
 
    public async Task AddGame(Game game, CancellationToken cancellationToken = default)
    {
        if (await ContainsGameAsync(game, cancellationToken))
            ThrowHelper.ThrowArgumentException($"Game \"{game}\" already added");
        await _dbContext.Games.AddAsync(game, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveGame(Game game, CancellationToken cancellationToken = default)
    {
        if (!await ContainsGameAsync(game, cancellationToken))
            ThrowHelper.ThrowArgumentException($"Game {game} not found");
        _dbContext.Remove(game);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public bool ContainsGame(Game game) => _dbContext.Games.Any(dbGame => dbGame.ProcessName == game.ProcessName);

    private readonly AppDbContext _dbContext;

    private Task<bool> ContainsGameAsync(Game game, CancellationToken cancellationToken) =>
        _dbContext.Games.AnyAsync(dbGame => dbGame.ProcessName == game.ProcessName, cancellationToken);
}