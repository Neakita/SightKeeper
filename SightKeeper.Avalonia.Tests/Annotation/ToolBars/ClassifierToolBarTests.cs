using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using CommunityToolkit.Diagnostics;
using FluentAssertions;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Tests.Annotation.ToolBars;

public sealed class ClassifierToolBarTests
{
	[AvaloniaFact]
	public void ListBoxShouldBeEmptyByDefault()
	{
		var (_, listBox) = PrepareToolBar();
		listBox.Items.Should().BeEmpty();
	}

	[AvaloniaFact]
	public void ListBoxShouldContainTagFromDataSet()
	{
		var (viewModel, listBox) = PrepareToolBar();
		var tag = SetDataSetWithTag(viewModel);
		listBox.Items.Should().Contain(tag);
	}

	[AvaloniaFact]
	public void ListBoxShouldBeDisabledWhenNoScreenshotSetInViewModel()
	{
		var (viewModel, listBox) = PrepareToolBar();
		SetDataSetWithTag(viewModel);
		listBox.IsEffectivelyEnabled.Should().BeFalse();
	}

	[AvaloniaFact]
	public void ListBoxShouldBeEnabled()
	{
		var (viewModel, listBox) = PrepareToolBar();
		SetDataSetWithTag(viewModel);
		viewModel.Screenshot = CreateScreenshot();
		listBox.IsEffectivelyEnabled.Should().BeTrue();
	}

	[AvaloniaFact]
	public void ListBoxShouldSetSelectedTag()
	{
		var (viewModel, listBox) = PrepareToolBar();
		var tag = SetDataSetWithTag(viewModel);
		viewModel.Screenshot = CreateScreenshot();
		listBox.Selection.Select(0);
		viewModel.SelectedTag.Should().Be(tag);
	}

	private static (ClassifierToolBarViewModel viewModel, ListBox listBox) PrepareToolBar()
	{
		ClassifierToolBarViewModel viewModel = CreateViewModel();
		ClassifierToolBar toolBar = new()
		{
			DataContext = viewModel
		};
		Window window = new()
		{
			Content = toolBar
		};
		window.Show();
		var listBox = GetListBox(toolBar);
		return (viewModel, listBox);
	}

	private static ClassifierToolBarViewModel CreateViewModel()
	{
		return new Composition().ClassifierAnnotationContext.ToolBar;
	}

	private static ListBox GetListBox(ClassifierToolBar toolBar)
	{
		var listBox = toolBar.FindDescendantOfType<ListBox>();
		Guard.IsNotNull(listBox);
		return listBox;
	}

	private static Tag SetDataSetWithTag(ClassifierToolBarViewModel viewModel)
	{
		var (dataSet, tag) = CreateDataSetWithTag();
		viewModel.DataSet = dataSet;
		return tag;
	}

	private static (ClassifierDataSet dataSet, Tag tag) CreateDataSetWithTag()
	{
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		return (dataSet, tag);
	}

	private static Screenshot CreateScreenshot()
	{
		ScreenshotsLibrary library = new();
		return library.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
	}
}