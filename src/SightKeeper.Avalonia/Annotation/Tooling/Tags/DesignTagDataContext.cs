namespace SightKeeper.Avalonia.Annotation.Tooling.Tags;

internal sealed class DesignTagDataContext : TagDataContext
{
	public string Name { get; }

	public DesignTagDataContext(string name)
	{
		Name = name;
	}
}