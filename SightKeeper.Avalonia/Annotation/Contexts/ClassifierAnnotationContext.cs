using System;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class ClassifierAnnotationContext : DataSetAnnotationContext, IDisposable
{
	public override ClassifierAnnotationViewModel ToolBar { get; }
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

	public ClassifierAnnotationContext(ClassifierAnnotationViewModel toolBar)
	{
		ToolBar = toolBar;
	}

	public void Dispose()
	{
		ToolBar.Dispose();
	}
}