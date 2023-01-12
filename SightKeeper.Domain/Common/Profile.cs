using SightKeeper.Domain.Common.Synergies;

namespace SightKeeper.Domain.Common;

public class Profile
{
	public Profile(string name)
	{
		Name = name;
		Components = new List<ProfileComponent>();
	}


	private Profile(int id, string name, string description)
	{
		Id = id;
		Name = name;
		Description = description;
		Game = null!;
		Components = null!;
	}

	public int Id { get; private set; }
	public string Name { get; set; }
	public string Description { get; set; } = string.Empty;
	public virtual Game? Game { get; set; }
	public virtual List<ProfileComponent> Components { get; private set; }
	public virtual Synergy? Synergy { get; set; }
}