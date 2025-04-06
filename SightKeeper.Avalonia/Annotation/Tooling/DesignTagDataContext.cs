namespace SightKeeper.Avalonia.Annotation.Tooling;

internal sealed class DesignTagDataContext : TagDataContext
{
	public string Name { get; }

	public DesignTagDataContext(string name)
	{
		Name = name;
	}
}