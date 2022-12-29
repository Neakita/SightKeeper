using System.ComponentModel.DataAnnotations;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.DAL.Domain.Common;

public class Profile
{
	public Profile(string name, string description, Game game, DetectorModel detectorModel)
	{
		Name = name;
		Description = description;
		Game = game;
		DetectorModel = detectorModel;
	}


	private Profile(int id, string name, string description)
	{
		Id = id;
		Name = name;
		Description = description;
		Game = null!;
		DetectorModel = null!;
	}

	public int Id { get; private set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public virtual Game Game { get; set; }
	public virtual DetectorModel DetectorModel { get; set; }
}