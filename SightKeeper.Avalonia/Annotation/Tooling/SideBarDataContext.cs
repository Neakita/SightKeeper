namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface SideBarDataContext
{
	ImageSetSelectionDataContext ImageSetSelection { get; }
	DataSetSelectionDataContext DataSetSelection { get; }
	ActionsDataContext Actions { get; }
	object? AdditionalTooling { get; }
}