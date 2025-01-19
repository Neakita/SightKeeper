using System;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Domain.DataSets.Classifier;
using ClassifierAnnotationViewModel = SightKeeper.Avalonia.Annotation.SideBars.ClassifierAnnotationViewModel;

namespace SightKeeper.Avalonia.Annotation.Contexts;

public sealed class ClassifierAnnotationContext : DataSetAnnotationContext, IDisposable
{
	public override ClassifierAnnotationViewModel SideBar { get; }
	public override DrawerViewModel? Drawer => null;

	public ClassifierDataSet? DataSet
	{
		get;
		set
		{
			field = value;
			SideBar.DataSet = value;
		}
	}

	public ClassifierAnnotationContext(ClassifierAnnotationViewModel sideBar)
	{
		SideBar = sideBar;
	}

	public void Dispose()
	{
		SideBar.Dispose();
	}
}