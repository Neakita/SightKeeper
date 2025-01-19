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
	public void ShouldShowTagSelectionToolBarForClassifierAnnotationViewModel()
	{
		var adaptiveToolBar = PrepareAdaptiveToolBar();
		adaptiveToolBar.DataContext = CreateClassifierAnnotationViewModel();
		AssertPresentsControl<TagSelectionToolBar>(adaptiveToolBar);
	}

	[AvaloniaFact]
	public void ShouldShowTagSelectionToolBarForTagSelectionViewModel()
	{
		var toolBar = PrepareAdaptiveToolBar();
		toolBar.DataContext = CreateTagSelectionViewModel();
		AssertPresentsControl<TagSelectionToolBar>(toolBar);
	}

	[AvaloniaFact]
	public void ShouldShowPoserToolBar()
	{
		var toolBar = PrepareAdaptiveToolBar();
		toolBar.DataContext = CreatePoserToolBarViewModel();
		AssertPresentsControl<PoserToolBar>(toolBar);
	}

	private static AdaptiveToolBar PrepareAdaptiveToolBar()
	{
		AdaptiveToolBar toolBar = new();
		Window window = new()
		{
			Content = toolBar
		};
		window.Show();
		return toolBar;
	}

	private static ClassifierAnnotationViewModel CreateClassifierAnnotationViewModel()
	{
		return new Composition().ClassifierAnnotationContext.ToolBar;
	}

	private static TagSelectionViewModel CreateTagSelectionViewModel()
	{
		return new TagSelectionViewModel();
	}

	private static PoserToolBarViewModel CreatePoserToolBarViewModel()
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