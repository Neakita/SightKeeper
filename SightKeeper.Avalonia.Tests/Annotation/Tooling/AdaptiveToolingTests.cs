using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using CommunityToolkit.Diagnostics;
using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Tooling;

namespace SightKeeper.Avalonia.Tests.Annotation.Tooling;

public sealed class AdaptiveToolingTests
{
	[AvaloniaFact]
	public void ShouldShowTagSelectionSideBarForClassifierAnnotationViewModel()
	{
		var adaptiveSideBar = PrepareAdaptiveSideBar();
		adaptiveSideBar.DataContext = CreateClassifierAnnotationViewModel();
		AssertPresentsControl<TagSelectionTooling>(adaptiveSideBar);
	}

	[AvaloniaFact]
	public void ShouldShowTagSelectionSideBarForTagSelectionViewModel()
	{
		var sideBar = PrepareAdaptiveSideBar();
		sideBar.DataContext = CreateTagSelectionViewModel();
		AssertPresentsControl<TagSelectionTooling>(sideBar);
	}

	[AvaloniaFact]
	public void ShouldShowPoserSideBar()
	{
		var sideBar = PrepareAdaptiveSideBar();
		sideBar.DataContext = CreatePoserSideBarViewModel();
		AssertPresentsControl<PoserSideBar>(sideBar);
	}

	private static AdaptiveTooling PrepareAdaptiveSideBar()
	{
		AdaptiveTooling sideBar = new();
		Window window = new()
		{
			Content = sideBar
		};
		window.Show();
		return sideBar;
	}

	private static ClassifierAnnotationViewModel CreateClassifierAnnotationViewModel()
	{
		return new Composition().ClassifierAnnotationViewModel;
	}

	private static TagSelectionViewModel CreateTagSelectionViewModel()
	{
		return new TagSelectionViewModel();
	}

	private static PoserSideBarViewModel CreatePoserSideBarViewModel()
	{
		return new PoserSideBarViewModel();
	}

	private static void AssertPresentsControl<T>(AdaptiveTooling sideBar)
	{
		var contentControl = sideBar.FindDescendantOfType<ContentControl>();
		Guard.IsNotNull(contentControl);
		var visual = contentControl.GetVisualDescendants().First(visual => visual is not ContentPresenter);
		visual.Should().BeOfType<T>();
	}

}