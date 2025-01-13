using FluentAssertions;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Avalonia.Tests.Annotation.ToolBars;

public sealed class ClassifierToolBarTests
{
	[Fact]
	public void TagsShouldBeEmptyByDefault()
	{
		ToolBar.Tags.Should().BeEmpty();
	}
	
	[Fact]
	public void ShouldSetTagsFromDataSet()
	{
		var toolBar = ToolBar;
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("Tag1");
		var tag2 = dataSet.TagsLibrary.CreateTag("Tag2");
		toolBar.DataSet = dataSet;
		toolBar.Tags.Should().Contain([tag1, tag2]);
	}

	[Fact]
	public void ShouldNotifyTagsChangeWhenDataSetChanged()
	{
		var subjectToolBar = ToolBar;
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("Tag");
		using var monitoredToolBar = subjectToolBar.Monitor();
		subjectToolBar.DataSet = dataSet;
		monitoredToolBar.Should().RaisePropertyChangeFor(toolBar => toolBar.Tags);
	}

	private static ClassifierToolBarViewModel ToolBar => new Composition().ClassifierAnnotationContext.ToolBar;
}