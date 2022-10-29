using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Abstract.Interfaces;
using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.DAL.Members.Common;

public sealed class Game : IGame
{
	public Guid Id { get; }
	public string Title { get; set; }
	public string ProcessName { get; }

	public List<Model> Models { get; private set; } = new();
	
	
	public Game(string title, string processName)
	{
		Title = title;
		ProcessName = processName;
	}


	private Game(Guid id, string title, string processName)
	{
		Id = id;
		Title = title;
		ProcessName = processName;
	}
}

internal class GameConfiguration : IEntityTypeConfiguration<Game>
{
	public void Configure(EntityTypeBuilder<Game> builder)
	{
		builder.HasKey(game => game.Id);
		builder.Property(game => game.ProcessName);
	}
}