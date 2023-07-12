using SightKeeper.Domain.Services;

namespace SightKeeper.Services.Games;

public sealed class RegisteredGamesService
{
    public RegisteredGamesService(GamesDataAccess gamesDataAccess, AvailableGamesProvider availableGamesProvider)
    {
        _gamesDataAccess = gamesDataAccess;
        _availableGamesProvider = availableGamesProvider;
    }
    
    public async Task<IReadOnlyCollection<GameDTO>> GetRegisteredGames(CancellationToken cancellationToken = default)
    {
        var games = await _gamesDataAccess.GetGames(cancellationToken);
        return games.Select(game => new GameDTO(game)).ToList();
    }

    public async Task<IReadOnlyCollection<GameDTO>> GetAvailableGames(CancellationToken cancellationToken = default)
    {
        var games = await _availableGamesProvider.GetAvailableGames(cancellationToken);
        return games.Select(game => new GameDTO(game)).ToList();
    }

    public bool CanRegisterGame(GameDTO game) =>
        !_gamesDataAccess.ContainsGame(game.Game);

    public Task RegisterGame(GameDTO game, CancellationToken cancellationToken = default) =>
        _gamesDataAccess.AddGame(game.Game, cancellationToken);

    public bool CanUnRegisterGame(GameDTO game) =>
        _gamesDataAccess.ContainsGame(game.Game);

    public Task UnRegisterGame(GameDTO game, CancellationToken cancellationToken = default) =>
        _gamesDataAccess.RemoveGame(game.Game, cancellationToken);

    private readonly GamesDataAccess _gamesDataAccess;
    private readonly AvailableGamesProvider _availableGamesProvider;
}