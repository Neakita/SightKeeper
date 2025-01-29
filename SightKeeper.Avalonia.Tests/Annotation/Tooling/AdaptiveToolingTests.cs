using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using CommunityToolkit.Diagnostics;
using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Domain.DataSets.Tags;

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
		sideBar.DataContext = CreatePoserToolingModel();
		AssertPresentsControl<PoserTooling>(sideBar);
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

	private static TagSelectionViewModel<Tag> CreateTagSelectionViewModel()
	{
		return new TagSelectionViewModel<Tag>();
	}

	private static PoserToolingViewModel CreatePoserToolingModel()
	{
		return new Composition().PoserToolingViewModel;
	}

	private static void AssertPresentsControl<T>(AdaptiveTooling sideBar)
	{
		var contentControl = sideBar.FindDescendantOfType<ContentControl>();
		Guard.IsNotNull(contentControl);
		var visual = contentControl.GetVisualDescendants().First(visual => visual is not ContentPresenter);
		visual.Should().BeOfType<T>();
	}

}