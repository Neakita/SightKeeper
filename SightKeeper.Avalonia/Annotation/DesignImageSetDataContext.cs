namespace SightKeeper.Avalonia.Annotation;

internal sealed class DesignImageSetDataContext : ImageSetDataContext
{
	public string Name { get; }

	public DesignImageSetDataContext(string name)
	{
		Name = name;
	}
}