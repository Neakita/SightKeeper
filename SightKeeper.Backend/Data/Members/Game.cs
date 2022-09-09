namespace SightKeeper.Backend.Data.Members;

public sealed class Game
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string ProcessName { get; set; } = string.Empty;
}
