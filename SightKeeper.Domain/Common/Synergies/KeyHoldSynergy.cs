namespace SightKeeper.Domain.Common.Synergies;

public sealed class KeyHoldSynergy : Synergy
{
	public KeyHoldSynergy(ProfileComponent component) : base(component)
	{
	}

	private KeyHoldSynergy(int id) : base(id)
	{
	}
	
	public int Key { get; set; }
}