using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.DAL.Members.Abstract;
using SightKeeper.DAL.Members.Abstract.Interfaces;

namespace SightKeeper.DAL.Members.Common;

/// <summary>
/// A class representing a simple game(process) with title and process name
/// </summary>
public sealed class Game : Abstract.Interfaces.Game
{
	public Guid Id { get; }
	
	/// <summary>
	/// Display name
	/// </summary>
	public string Title { get; set; }
	
	/// <summary>
	/// System process name
	/// </summary>
	public string ProcessName { get; }

	/// <summary>
	/// Dependent models
	/// </summary>
	public List<Model>? Models { get; private set; } = new();
	
	
	public Game(string title, string processName)
	{
		Title = title;
		ProcessName = processName;
	}


	// ReSharper disable once UnusedMember.Local
	/// <summary>
	/// Constructor used by Entity Framework
	/// </summary>
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