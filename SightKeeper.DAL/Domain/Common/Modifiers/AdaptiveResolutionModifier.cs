namespace SightKeeper.DAL.Domain.Common.Modifiers;

public sealed class AdaptiveResolutionModifier : Modifier
{
	public AdaptiveResolutionModifier(ProfileComponent component) : base(component)
	{
	}

	private AdaptiveResolutionModifier(int id) : base(id)
	{
	}
}
