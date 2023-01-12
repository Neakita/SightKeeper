//using SharpHook;

namespace SightKeeper.Domain.Common.Synergies;

public sealed class SingleKeySwitchSynergy : Synergy
{
	public SingleKeySwitchSynergy(ProfileComponent component) : base(component)
	{
	}

	private SingleKeySwitchSynergy(int id) : base(id)
	{
		
	}
	
	//public Keys? Key { get; set; }
}
