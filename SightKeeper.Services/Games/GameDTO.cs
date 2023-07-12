using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Services.Games;

public sealed class GameDTO
{
    public Game Game { get; }
    public string Title { get; }
    public string ProcessName { get; }
    
    public GameDTO(Game game)
    {
        Game = game;
        Title = game.Title;
        ProcessName = game.ProcessName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((GameDTO)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Title, ProcessName);
    }

    private bool Equals(GameDTO other) => ProcessName == other.ProcessName;
}