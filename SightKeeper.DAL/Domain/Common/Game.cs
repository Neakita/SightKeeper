using System.ComponentModel.DataAnnotations;
using SightKeeper.DAL.Domain.Abstract;

namespace SightKeeper.DAL.Domain.Common;

/// <summary>
///     A class representing a game (process) with title and process name
/// </summary>
public class Game
{
	public Game(string title, string processName)
	{
		Title = title;
		ProcessName = processName;
	}


	// ReSharper disable once UnusedMember.Local
	/// <summary>
	///     Constructor used by Entity Framework
	/// </summary>
	private Game(int id, string title, string processName)
	{
		Id = id;
		Title = title;
		ProcessName = processName;
	}

	[Key] public int Id { get; }

	/// <summary>
	///     Display name
	/// </summary>
	public string Title { get; set; }

	/// <summary>
	///     System process name
	/// </summary>
	public string ProcessName { get; }

	/// <summary>
	///     Dependent models
	/// </summary>
	public virtual List<Model> Models { get; } = new();
}