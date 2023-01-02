using System.Windows.Forms;

namespace SightKeeper.DAL.Domain.Common.Synergies;

public sealed class KeyHoldSynergy : Synergy
{
	public KeyHoldSynergy(ProfileComponent component) : base(component)
	{
	}

	private KeyHoldSynergy(int id) : base(id)
	{
	}
	
	public Keys? Key { get; set; }
}