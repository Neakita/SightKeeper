using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.DAL.Members.Common;

public sealed class Game : Entity
{
	public string Title { get; set; }
	public string ProcessName { get; }

	public ICollection<Model> Models { get; } = new List<Model>();
	

	public Game(string title, string processName)
	{
		Title = title;
		ProcessName = processName;
	}
	
	
	private Game(Guid id, string title, string processName) : base(id)
	{
		Title = title;
		ProcessName = processName;
	}
}

internal class GameConfiguration : IEntityTypeConfiguration<Game>
{
	public void Configure(EntityTypeBuilder<Game> builder) =>
		builder.Property(game => game.ProcessName);
}