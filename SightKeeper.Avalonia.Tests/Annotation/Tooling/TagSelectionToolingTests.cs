using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using CommunityToolkit.Diagnostics;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Tests.Annotation.Tooling;

public sealed class TagSelectionToolingTests
{
	[AvaloniaFact]
	public void ShouldBeEmptyByDefault()
	{
		var dataContext = Substitute.For<TagSelectionToolingDataContext>();
		dataContext.Tags.Returns(ReadOnlyCollection<Named>.Empty);
		var tooling = PrepareTooling(dataContext);
		var selectionControl = GetSelectionControl(tooling); 
		selectionControl.Items.Should().BeEmpty();
	}

	[AvaloniaFact]
	public void ShouldDisplayTag()
	{
		var dataContext = Substitute.For<TagSelectionToolingDataContext>();
		var tag = Substitute.For<Named>();
		dataContext.Tags.Returns([tag]);
		var tooling = PrepareTooling(dataContext);
		var selectionControl = GetSelectionControl(tooling);
		selectionControl.Items.Should().Contain(tag);
	}

	[AvaloniaFact]
	public void ShouldSetSelectedTag()
	{
		var dataContext = Substitute.For<TagSelectionToolingDataContext>();
		var tag = Substitute.For<Named>();
		dataContext.Tags.Returns([tag]);
		var tooling = PrepareTooling(dataContext);
		var selectionControl = GetSelectionControl(tooling);
		selectionControl.SelectedItem = tag;
		dataContext.SelectedTag.Should().Be(tag);
	}

	[AvaloniaFact]
	public void ShouldClearSelectedTag()
	{
		var dataContext = Substitute.For<TagSelectionToolingDataContext>();
		var tag = Substitute.For<Named>();
		dataContext.Tags.Returns([tag]);
		var tooling = PrepareTooling(dataContext);
		var selectionControl = GetSelectionControl(tooling);
		selectionControl.SelectedItem = tag;
		dataContext.Tags.Returns(ReadOnlyCollection<Named>.Empty);
		dataContext.PropertyChanged += Raise.Event<PropertyChangedEventHandler>(new PropertyChangedEventArgs(nameof(TagSelectionToolingDataContext.Tags)));
		dataContext.Received().SelectedTag = null;
	}

	private static TagSelectionTooling PrepareTooling(TagSelectionToolingDataContext dataContext)
	{
		TagSelectionTooling tooling = new()
		{
			DataContext = dataContext
		};
		Window window = new()
		{
			Content = tooling
		};
		window.Show();
		return tooling;
	}

	private static SelectingItemsControl GetSelectionControl(TagSelectionTooling tooling)
	{
		var control = tooling.FindDescendantOfType<SelectingItemsControl>();
		Guard.IsNotNull(control);
		return control;

	}
}