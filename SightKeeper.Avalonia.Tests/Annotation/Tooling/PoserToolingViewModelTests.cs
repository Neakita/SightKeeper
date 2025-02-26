using System.Reactive.Subjects;
using System.Windows.Input;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.Annotation;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Tests.Annotation.Tooling;

public sealed class PoserToolingViewModelTests
{
	[Fact]
	public void TagSelectionShouldContainTagsFromProvidedSource()
	{
		var tooling = CreateTooling();
		var tag1 = CreatePoserTag();
		var tag2 = CreatePoserTag();
		var tagsContainer = Substitute.For<TagsContainer<PoserTag>>();
		tagsContainer.Tags.Returns([tag1, tag2]);
		tooling.TagsSource = tagsContainer;
		tooling.TagSelection.Tags.Should().Contain(tagsContainer.Tags);
	}

	[Fact]
	public void SelectionShouldGetKeyPointTagsFromProvidedItem()
	{
		var screenshot = CreateScreenshot();
		var (keyPointTag, item) = CreateKeyPointTagAndPoserItem(screenshot);
		var tooling = CreateTooling();
		tooling.SelectedItem = item;
		tooling.KeyPointTagSelection.Tags.Should().Contain(keyPointTag);
	}

	[Fact]
	public void ShouldDeleteKeyPointByTag()
	{
		var screenshot = CreateScreenshot();
		var (keyPointTag, item) = CreateKeyPointTagAndPoserItem(screenshot);
		var keyPoint = CreateKeyPoint(item, keyPointTag);
		var tooling = CreateTooling();
		tooling.SelectedItem = item;
		tooling.DeleteKeyPointCommand.Execute(keyPointTag);
		item.KeyPoints.Should().NotContain(keyPoint);
	}

	[Fact]
	public void DeleteKeyPointByTagCommandCanExecuteShouldBeTrue()
	{
		var screenshot = CreateScreenshot();
		var (keyPointTag, item) = CreateKeyPointTagAndPoserItem(screenshot);
		CreateKeyPoint(item, keyPointTag);
		var tooling = CreateTooling();
		tooling.SelectedItem = item;
		tooling.DeleteKeyPointCommand.CanExecute(keyPointTag).Should().BeTrue();
	}

	[Fact]
	public void DeleteKeyPointCommandCanExecuteShouldBeFalseWhenNoKeyPointWithSuchTagExist()
	{
		var screenshot = CreateScreenshot();
		var (keyPointTag, item) = CreateKeyPointTagAndPoserItem(screenshot);
		var tooling = CreateTooling();
		tooling.SelectedItem = item;
		tooling.DeleteKeyPointCommand.CanExecute(keyPointTag).Should().BeFalse();
	}

	[Fact]
	public void DeleteKeyPointCommandShouldRaiseCanExecuteChangedWhenKeyPointCreated()
	{
		var screenshot = CreateScreenshot();
		var (keyPointTag, item) = CreateKeyPointTagAndPoserItem(screenshot);
		var tooling = CreateTooling(out var keyPointCreatedSubject);
		tooling.SelectedItem = item;
		using var commandMonitor = tooling.DeleteKeyPointCommand.Monitor();
		var keyPoint = CreateKeyPoint(item, keyPointTag);
		keyPointCreatedSubject.OnNext((item, keyPoint));
		commandMonitor.Should().Raise(nameof(ICommand.CanExecuteChanged));
	}

	[Fact]
	public void DeleteKeyPointCommandShouldRaiseCanExecuteChangedWhenSelectedItemChanged()
	{
		var screenshot = CreateScreenshot();
		var (keyPointTag, item) = CreateKeyPointTagAndPoserItem(screenshot);
		CreateKeyPoint(item, keyPointTag);
		var tooling = CreateTooling();
		using var commandMonitor = tooling.DeleteKeyPointCommand.Monitor();
		tooling.SelectedItem = item;
		commandMonitor.Should().Raise(nameof(ICommand.CanExecuteChanged));
	}

	private static Screenshot CreateScreenshot()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		return screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
	}

	private static (Tag keyPointTag, Poser2DItem item) CreateKeyPointTagAndPoserItem(Screenshot screenshot)
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var keyPointTag = tag.CreateKeyPointTag("TestKeyPointTag");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item = asset.CreateItem(tag, new Bounding(0.1, 0.2, 0.3, 0.4));
		return (keyPointTag, item);
	}

	private static PoserToolingViewModel CreateTooling(out Subject<(PoserItem item, KeyPoint keyPoint)> keyPointCreatedSubject)
	{
		keyPointCreatedSubject = new Subject<(PoserItem item, KeyPoint keyPoint)>();
		var observablePoserAnnotator = Substitute.For<ObservablePoserAnnotator>();
		observablePoserAnnotator.KeyPointCreated.Returns(keyPointCreatedSubject);
		return new PoserToolingViewModel(Substitute.For<PoserAnnotator>(), observablePoserAnnotator);
	}

	private static PoserToolingViewModel CreateTooling()
	{
		return new Composition().PoserToolingViewModel;
	}

	private static KeyPoint CreateKeyPoint(Poser2DItem item, Tag keyPointTag)
	{
		return item.CreateKeyPoint(keyPointTag, new Vector2<double>(0.12, 0.34));
	}

	private static PoserTag CreatePoserTag()
	{
		Poser2DDataSet dataSet = new();
		return dataSet.TagsLibrary.CreateTag(string.Empty);
	}
}