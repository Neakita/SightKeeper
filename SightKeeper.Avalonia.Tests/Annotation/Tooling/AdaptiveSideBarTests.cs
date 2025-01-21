using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using CommunityToolkit.Diagnostics;
using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Tooling;

namespace SightKeeper.Avalonia.Tests.Annotation.Tooling;

public sealed class AdaptiveSideBarTests
{
	[AvaloniaFact]
	public void ShouldShowTagSelectionSideBarForClassifierAnnotationViewModel()
	{
		var adaptiveSideBar = PrepareAdaptiveSideBar();
		adaptiveSideBar.DataContext = CreateClassifierAnnotationViewModel();
		AssertPresentsControl<TagSelectionSideBar>(adaptiveSideBar);
	}

	[AvaloniaFact]
	public void ShouldShowTagSelectionSideBarForTagSelectionViewModel()
	{
		var sideBar = PrepareAdaptiveSideBar();
		sideBar.DataContext = CreateTagSelectionViewModel();
		AssertPresentsControl<TagSelectionSideBar>(sideBar);
	}

	[AvaloniaFact]
	public void ShouldShowPoserSideBar()
	{
		var sideBar = PrepareAdaptiveSideBar();
		sideBar.DataContext = CreatePoserSideBarViewModel();
		AssertPresentsControl<PoserSideBar>(sideBar);
	}

	private static AdaptiveSideBar PrepareAdaptiveSideBar()
	{
		AdaptiveSideBar sideBar = new();
		Window window = new()
		{
			Content = sideBar
		};
		window.Show();
		return sideBar;
	}

	private static ClassifierAnnotationViewModel CreateClassifierAnnotationViewModel()
	{
		return new Composition().ClassifierAnnotationContext.SideBar;
	}

	private static TagSelectionViewModel CreateTagSelectionViewModel()
	{
		return new TagSelectionViewModel();
	}

	private static PoserSideBarViewModel CreatePoserSideBarViewModel()
	{
		return new PoserSideBarViewModel();
	}

	private static void AssertPresentsControl<T>(AdaptiveSideBar sideBar)
	{
		var contentControl = sideBar.FindDescendantOfType<ContentControl>();
		Guard.IsNotNull(contentControl);
		var visual = contentControl.GetVisualDescendants().First(visual => visual is not ContentPresenter);
		visual.Should().BeOfType<T>();
	}

}