using System.Windows.Forms;

namespace SightKeeper.DAL.Domain.Common.Synergies;

public sealed class KeyWrapper
{
	public int Id { get; private set; }
	public Keys Key { get; set; }
}