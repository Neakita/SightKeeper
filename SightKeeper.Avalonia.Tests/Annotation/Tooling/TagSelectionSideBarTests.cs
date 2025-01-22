using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using CommunityToolkit.Diagnostics;
using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Tests.Annotation.Tooling;

public sealed class TagSelectionToolingTests
{
	[AvaloniaFact]
	public void ShouldBeEmptyByDefault()
	{
		var (_, listBox) = PrepareSideBar();
		listBox.Items.Should().BeEmpty();
	}

	[AvaloniaFact]
	public void ShouldContainTagViewModel()
	{
		var (viewModel, listBox) = PrepareSideBar();
		var tag = CreateDataSetWithTag();
		viewModel.Tags = [tag];
		listBox.Items.Should().Contain(tag);
	}

	[AvaloniaFact]
	public void ShouldSetSelectedTag()
	{
		var (viewModel, listBox) = PrepareSideBar();
		var tag = CreateDataSetWithTag();
		viewModel.Tags = [tag];
		listBox.Selection.Select(0);
		viewModel.SelectedTag.Should().Be(tag);
	}

	[AvaloniaFact]
	public void ShouldClearSelectedTag()
	{
		var (viewModel, listBox) = PrepareSideBar();
		var tag = CreateDataSetWithTag();
		viewModel.Tags = [tag];
		listBox.Selection.Select(0);
		viewModel.Tags = ReadOnlyCollection<Tag>.Empty;
		viewModel.SelectedTag.Should().BeNull();
	}

	private static (TagSelectionViewModel viewModel, ListBox listBox) PrepareSideBar()
	{
		TagSelectionViewModel viewModel = new();
		TagSelectionTooling tooling = new()
		{
			DataContext = viewModel
		};
		Window window = new()
		{
			Content = tooling
		};
		window.Show();
		var listBox = tooling.FindDescendantOfType<ListBox>();
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