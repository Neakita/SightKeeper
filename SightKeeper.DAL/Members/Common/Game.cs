using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.DAL.Members.Common;

/// <summary>
/// A class representing a game (process) with title and process name
/// </summary>
public class Game
{
	[Key] public Guid Id { get; private set; }
	
	/// <summary>
	/// Display name
	/// </summary>
	public string Title { get; set; }
	
	/// <summary>
	/// System process name
	/// </summary>
	public string ProcessName { get; private set; }

	/// <summary>
	/// Dependent models
	/// </summary>
	public virtual List<Model> Models { get; private set; } = new();
	
	
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