namespace SightKeeper.DAL.Domain.Common.Modifiers;

public sealed class ResolutionMultiplierModifier : Modifier
{
	public ResolutionMultiplierModifier(ProfileComponent component) : base(component)
	{
	}

	private ResolutionMultiplierModifier(int id) : base(id)
	{
	}
	
	public int ResolutionMultiplier { get; set; } = 2;
}
