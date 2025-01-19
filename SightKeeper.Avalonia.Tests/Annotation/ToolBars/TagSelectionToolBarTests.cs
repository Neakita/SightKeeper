using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using CommunityToolkit.Diagnostics;
using FluentAssertions;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Tests.Annotation.ToolBars;

public sealed class TagSelectionToolBarTests
{
	[AvaloniaFact]
	public void ShouldBeEmptyByDefault()
	{
		var (_, listBox) = PrepareToolBar();
		listBox.Items.Should().BeEmpty();
	}

	[AvaloniaFact]
	public void ShouldContainTagViewModel()
	{
		var (viewModel, listBox) = PrepareToolBar();
		var tag = CreateDataSetWithTag();
		viewModel.Tags = [tag];
		listBox.Items.Should().Contain(tag);
	}

	[AvaloniaFact]
	public void ShouldSetSelectedTag()
	{
		var (viewModel, listBox) = PrepareToolBar();
		var tag = CreateDataSetWithTag();
		viewModel.Tags = [tag];
		listBox.Selection.Select(0);
		viewModel.SelectedTag.Should().Be(tag);
	}

	[AvaloniaFact]
	public void ShouldClearSelectedTag()
	{
		var (viewModel, listBox) = PrepareToolBar();
		var tag = CreateDataSetWithTag();
		viewModel.Tags = [tag];
		listBox.Selection.Select(0);
		viewModel.Tags = ReadOnlyCollection<Tag>.Empty;
		viewModel.SelectedTag.Should().BeNull();
	}

	private static (TagSelectionViewModel viewModel, ListBox listBox) PrepareToolBar()
	{
		TagSelectionViewModel viewModel = new();
		TagSelectionToolBar toolBar = new()
		{
			DataContext = viewModel
		};
		Window window = new()
		{
			Content = toolBar
		};
		window.Show();
		var listBox = toolBar.FindDescendantOfType<ListBox>();
		Guard.IsNotNull(listBox);
		return (viewModel, listBox);
	}

	private static Tag CreateDataSetWithTag()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		return tag;
	}
}