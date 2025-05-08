using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using SightKeeper.Avalonia.DataSets;

namespace SightKeeper.Avalonia.Annotation.Tooling;

internal sealed class DesignSideBarDataContext : SideBarDataContext, DataSetSelectionDataContext
{
	public IReadOnlyCollection<ImageSetDataContext> ImageSets =>
	[
		new DesignImageSetDataContext("Image Set 1"),
		new DesignImageSetDataContext("Image Set 2")
	];

	public ImageSetDataContext? SelectedImageSet { get; set; }

	public IReadOnlyCollection<DataSetDataContext> DataSets =>
	[
		new DesignDataSetDataContext("Data Set 1"),
		new DesignDataSetDataContext("Data Set 2")
	];

	public DataSetDataContext? SelectedDataSet { get; set; }

	public DataSetSelectionDataContext DataSetSelection => this;

	public IReadOnlyCollection<AnnotationButtonDefinition> ButtonDefinitions => Enumerable.Repeat(
		new AnnotationButtonDefinition
		{
			IconKind = MaterialIconKind.Abacus,
			Command = new RelayCommand(() => { }),
			ToolTip = "The tool tip"
		}, 5).ToList();

	public object? AdditionalTooling => null;
}