using System.Collections.Generic;
using SightKeeper.Avalonia.DataSets;

namespace SightKeeper.Avalonia.Annotation.Tooling;

internal sealed class DesignSideBarDataContext : SideBarDataContext, DataSetSelectionDataContext
{
	public ImageSetSelectionDataContext ImageSetSelection => new DesignImageSelectionDataContext();

	public IReadOnlyCollection<DataSetDataContext> DataSets =>
	[
		new DesignDataSetDataContext("Data Set 1"),
		new DesignDataSetDataContext("Data Set 2")
	];

	public DataSetDataContext? SelectedDataSet { get; set; }

	public DataSetSelectionDataContext DataSetSelection => this;

	public ActionsDataContext Actions => new DesignActionsDataContext();

	public object? AdditionalTooling => null;
}