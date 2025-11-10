using System.Collections.Generic;
using SightKeeper.Avalonia.Annotation.Tooling.Actions;
using SightKeeper.Avalonia.Annotation.Tooling.DataSet;
using SightKeeper.Avalonia.Annotation.Tooling.ImageSet;
using SightKeeper.Avalonia.DataSets;

namespace SightKeeper.Avalonia.Annotation.Tooling;

internal sealed class DesignSideBarDataContext : SideBarDataContext, DataSetSelectionDataContext
{
	public ImageSetSelectionDataContext ImageSetSelection => new DesignImageSetSelectionDataContext();

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