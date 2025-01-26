using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Avalonia.Annotation.Tooling;

namespace SightKeeper.Avalonia.Tests.Annotation.Tooling;

public sealed class PoserToolingTests
{
	[AvaloniaFact]
	public void ShouldContainTagSelection()
	{
		var poserToolingDataContext = Substitute.For<PoserToolingDataContext>();
		var tagSelection = Substitute.For<TagSelectionToolingDataContext>();
		poserToolingDataContext.TagSelection.Returns(tagSelection);
		PoserTooling poserTooling = new()
		{
			DataContext = poserToolingDataContext
		};
		Window window = new()
		{
			Content = poserTooling
		};
		window.Show();
		poserTooling.GetVisualDescendants().Should().ContainSingle(descendant => descendant is SelectingItemsControl && descendant.DataContext == tagSelection);
	}

	[AvaloniaFact]
	public void ShouldContainKeyPointTagSelection()
	{
		var poserToolingDataContext = Substitute.For<PoserToolingDataContext>();
		var tagSelection = Substitute.For<TagSelectionToolingDataContext>();
		poserToolingDataContext.KeyPointTagSelection.Returns(tagSelection);
		PoserTooling poserTooling = new()
		{
			DataContext = poserToolingDataContext
		};
		Window window = new()
		{
			Content = poserTooling
		};
		window.Show();
		poserTooling.GetVisualDescendants().Should().ContainSingle(descendant => descendant is SelectingItemsControl && descendant.DataContext == tagSelection);
	}
}