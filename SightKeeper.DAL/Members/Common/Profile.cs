using System.ComponentModel.DataAnnotations;
using SightKeeper.DAL.Members.Detector;

namespace SightKeeper.DAL.Members.Common;

public class Profile
{
	[Key] public Guid Id { get; private set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public virtual Game Game { get; set; }
	public virtual DetectorModel DetectorModel { get; set; }


	public Profile(string name, string description, Game game, DetectorModel detectorModel)
	{
		Name = name;
		Description = description;
		Game = game;
		DetectorModel = detectorModel;
	}
	
	
	private Profile(Guid id, string name, string description)
	{
		Id = id;
		Name = name;
		Description = description;
		Game = null!;
		DetectorModel = null!;
	}
}