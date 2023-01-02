namespace SightKeeper.DAL.Domain.Common.Modifiers;

public sealed class AdaptiveAreaModifier : Modifier
{
	public AdaptiveAreaModifier(ProfileComponent component) : base(component)
	{
		AreaWidth = 640;
		AreaHeight = 640;
	}

	private AdaptiveAreaModifier(int id) : base(id)
	{
	}
	
	public int AreaWidth { get; set; }
	public int AreaHeight { get; set; }
	
	public int HorizontalOffset { get; set; }
	public int VerticalOffset { get; set; }
}