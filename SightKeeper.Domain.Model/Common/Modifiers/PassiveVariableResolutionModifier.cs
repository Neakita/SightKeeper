namespace SightKeeper.Domain.Model.Common.Modifiers;

public sealed class PassiveVariableResolutionModifier : Modifier
{
	public PassiveVariableResolutionModifier(ProfileComponent component) : base(component)
	{
		ScansPerStep = 1;
		Step = 32;
		MinimumStep = -2;
		MaximumStep = 2;
	}

	private PassiveVariableResolutionModifier(int id) : base(id)
	{
	}
	
	public byte ScansPerStep { get; set; }
	public ushort Step { get; set; }
	public short MinimumStep { get; set; }
	public short MaximumStep { get; set; }
	
}
