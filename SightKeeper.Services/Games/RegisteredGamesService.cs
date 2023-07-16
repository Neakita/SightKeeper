using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services.Games;

public sealed class RegisteredGamesService
{
    public RegisteredGamesService(GamesDataAccess gamesDataAccess, AvailableGamesProvider availableGamesProvider)
    {
        _gamesDataAccess = gamesDataAccess;
        _availableGamesProvider = availableGamesProvider;
    }
    
    public Task<IReadOnlyCollection<Game>> GetRegisteredGames(CancellationToken cancellationToken = default)
    {
        return _gamesDataAccess.GetGames(cancellationToken);
    }

    public Task<IReadOnlyCollection<Game>> GetAvailableGames(CancellationToken cancellationToken = default) =>
        _availableGamesProvider.GetAvailableGames(cancellationToken);

    public bool CanRegisterGame(Game game) =>
        !_gamesDataAccess.ContainsGame(game);

    public Task RegisterGame(Game game, CancellationToken cancellationToken = default) =>
        _gamesDataAccess.AddGame(game, cancellationToken);

    public bool CanUnRegisterGame(Game game) =>
        _gamesDataAccess.ContainsGame(game);

    public Task UnRegisterGame(Game game, CancellationToken cancellationToken = default) =>
        _gamesDataAccess.RemoveGame(game, cancellationToken);

    private readonly GamesDataAccess _gamesDataAccess;
    private readonly AvailableGamesProvider _availableGamesProvider;
}