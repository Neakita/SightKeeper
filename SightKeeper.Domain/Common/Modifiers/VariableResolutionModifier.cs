//using System.Windows.Forms;

namespace SightKeeper.Domain.Common.Modifiers;

public sealed class VariableResolutionModifier : Modifier
{
	public VariableResolutionModifier(ProfileComponent component) : base(component)
	{
		ChangeStep = 32;
	}

	private VariableResolutionModifier(int id) : base(id)
	{
	}
	
	public ushort ChangeStep { get; set; }
	
	//public Keys? IncreaseResolutionKey { get; set; }
	//public Keys? DecreaseResolutionKey { get; set; }
}