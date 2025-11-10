using SightKeeper.Avalonia.Annotation.Tooling.Actions;
using SightKeeper.Avalonia.Annotation.Tooling.DataSet;
using SightKeeper.Avalonia.Annotation.Tooling.ImageSet;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface SideBarDataContext
{
	ImageSetSelectionDataContext ImageSetSelection { get; }
	DataSetSelectionDataContext DataSetSelection { get; }
	ActionsDataContext Actions { get; }
	object? AdditionalTooling { get; }
}