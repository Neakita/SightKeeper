using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling.Tags;

public interface TagSelection
{
	Tag? SelectedTag { get; }
}