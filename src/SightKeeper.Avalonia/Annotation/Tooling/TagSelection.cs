using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface TagSelection
{
	Tag? SelectedTag { get; }
}