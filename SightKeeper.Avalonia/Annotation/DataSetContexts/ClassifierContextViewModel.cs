using System;
using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal sealed class ClassifierContextViewModel : DataSetContextViewModel<ClassifierAssetViewModel, ClassifierAsset>, IDisposable
{
	public override ClassifierToolBarViewModel ToolBar { get; }
	public override DrawerViewModel? Drawer => null;

	public ClassifierContextViewModel(
		ClassifierDataSet dataSet,
		ClassifierAnnotator classifierAnnotator)
	{
		ToolBar = new ClassifierToolBarViewModel(dataSet.TagsLibrary.Tags, classifierAnnotator);
	}

	public void Dispose()
	{
	}
}