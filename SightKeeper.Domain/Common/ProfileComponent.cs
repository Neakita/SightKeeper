using SightKeeper.Domain.Abstract;
using SightKeeper.Domain.Common.Modifiers;

namespace SightKeeper.Domain.Common;

public sealed class ProfileComponent
{
	public ProfileComponent(Profile profile, Model model)
	{
		Profile = profile;
		Model = model;
		ItemClassesGroups = new List<ItemClassGroup>();
		Modifiers = new List<Modifier>();
	}
	
	private ProfileComponent(int id)
	{
		Id = id;
		Model = null!;
		Profile = null!;
		ItemClassesGroups = null!;
		Modifiers = null!;
	}
	
	
	public int Id { get; private set; }
	public Model Model { get; set; }
	public Profile Profile { get; private set; }
	public List<ItemClassGroup> ItemClassesGroups { get; private set; }
	public List<Modifier> Modifiers { get; private set; }
}