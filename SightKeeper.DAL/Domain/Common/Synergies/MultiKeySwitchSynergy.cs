namespace SightKeeper.DAL.Domain.Common.Synergies;

public sealed class MultiKeySwitchSynergy : Synergy
{
	private MultiKeySwitchSynergy(int id) : base(id)
	{
		Keys = null!;
	}

	public MultiKeySwitchSynergy(ProfileComponent component) : base(component)
	{
		Keys = new List<KeyWrapper>();
	}
	
	
	public List<KeyWrapper> Keys { get; private set; }
}
