using System;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal sealed class ClassifierAnnotationContextViewModel : DataSetAnnotationContextViewModel, IDisposable
{
	public override ClassifierToolBarViewModel ToolBar { get; }
	public override DrawerViewModel? Drawer => null;

	public ClassifierDataSet? DataSet
	{
		get;
		set
		{
			field = value;
			ToolBar.DataSet = value;
		}
	}

	public ClassifierAnnotationContextViewModel(ClassifierToolBarViewModel toolBar)
	{
		ToolBar = toolBar;
	}

	public void Dispose()
	{
		ToolBar.Dispose();
	}
}