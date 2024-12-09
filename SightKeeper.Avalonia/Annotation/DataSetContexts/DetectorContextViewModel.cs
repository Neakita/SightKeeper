using System.ComponentModel;
using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal sealed class DetectorContextViewModel : DataSetContextViewModel<DetectorAssetViewModel, DetectorAsset>
{
	public override DetectorToolBarViewModel ToolBar { get; }
	public override DetectorDrawerViewModel Drawer { get; }

	public DetectorContextViewModel(
		DetectorDataSet dataSet,
		DetectorAnnotator detectorAnnotator)
	{
		ToolBar = new DetectorToolBarViewModel(dataSet.TagsLibrary.Tags);
		Drawer = new DetectorDrawerViewModel(detectorAnnotator);
		ToolBar.PropertyChanged += OnToolBarPropertyChanged;
	}

	private void OnToolBarPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(ToolBar.Tag))
			Drawer.SetTag(ToolBar.Tag);
	}
}