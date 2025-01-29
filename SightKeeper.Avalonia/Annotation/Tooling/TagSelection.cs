namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface TagSelection<out TTag>
{
	TTag? SelectedTag { get; }
}