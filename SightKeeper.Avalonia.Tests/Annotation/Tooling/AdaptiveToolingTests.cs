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
	public void ShouldShowTagSelectionToolingForClassifierAnnotationViewModel()
	{
		var adaptiveTooling = PrepareAdaptiveTooling();
		adaptiveTooling.DataContext = CreateClassifierAnnotationViewModel();
		AssertPresentsControl<TagSelectionTooling>(adaptiveTooling);
	}

	[AvaloniaFact]
	public void ShouldShowTagSelectionToolingForTagSelectionViewModel()
	{
		var adaptiveTooling = PrepareAdaptiveTooling();
		adaptiveTooling.DataContext = CreateTagSelectionViewModel();
		AssertPresentsControl<TagSelectionTooling>(adaptiveTooling);
	}

	[AvaloniaFact]
	public void ShouldShowPoserTooling()
	{
		var adaptiveTooling = PrepareAdaptiveTooling();
		adaptiveTooling.DataContext = CreatePoserToolingModel();
		AssertPresentsControl<PoserTooling>(adaptiveTooling);
	}

	private static AdaptiveTooling PrepareAdaptiveTooling()
	{
		AdaptiveTooling tooling = new();
		Window window = new()
		{
			Content = tooling
		};
		window.Show();
		return tooling;
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