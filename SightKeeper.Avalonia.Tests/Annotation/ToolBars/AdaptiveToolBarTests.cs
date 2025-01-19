using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using CommunityToolkit.Diagnostics;
using FluentAssertions;
using SightKeeper.Avalonia.Annotation.ToolBars;

namespace SightKeeper.Avalonia.Tests.Annotation.ToolBars;

public sealed class AdaptiveToolBarTests
{
	[AvaloniaFact]
	public void ShouldShowClassifierToolBar()
	{
		var toolBar = PrepareToolBar();
		toolBar.DataContext = CreateClassifierToolBarViewModel();
		AssertPresentsControl<ClassifierToolBar>(toolBar);
	}

	[AvaloniaFact]
	public void ShouldShowDetectorToolBar()
	{
		var toolBar = PrepareToolBar();
		toolBar.DataContext = CreateDetectorToolBarViewModel();
		AssertPresentsControl<DetectorToolBar>(toolBar);
	}

	[AvaloniaFact]
	public void ShouldShowPoserToolBar()
	{
		var toolBar = PrepareToolBar();
		toolBar.DataContext = CreatePoserToolBarViewModel();
		AssertPresentsControl<PoserToolBar>(toolBar);
	}

	private static AdaptiveToolBar PrepareToolBar()
	{
		AdaptiveToolBar toolBar = new();
		Window window = new()
		{
			Content = toolBar
		};
		window.Show();
		return toolBar;
	}

	private static ClassifierToolBarViewModel CreateClassifierToolBarViewModel()
	{
		return new Composition().ClassifierAnnotationContext.ToolBar;
	}

	private DetectorToolBarViewModel CreateDetectorToolBarViewModel()
	{
		return new DetectorToolBarViewModel();
	}

	private PoserToolBarViewModel CreatePoserToolBarViewModel()
	{
		return new PoserToolBarViewModel();
	}

	private static void AssertPresentsControl<T>(AdaptiveToolBar toolBar)
	{
		var contentControl = toolBar.FindDescendantOfType<ContentControl>();
		Guard.IsNotNull(contentControl);
		var visual = contentControl.GetVisualDescendants().First(visual => visual is not ContentPresenter);
		visual.Should().BeOfType<T>();
	}

}