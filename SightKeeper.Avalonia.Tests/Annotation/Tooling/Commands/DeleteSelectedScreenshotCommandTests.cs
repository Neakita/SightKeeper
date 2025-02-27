using System.Reactive;
using System.Reactive.Subjects;
using System.Windows.Input;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.Annotation;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.Tooling.Commands;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Tests.Annotation.Tooling.Commands;

public sealed class DeleteSelectedScreenshotCommandTests
{
	[Fact]
	public void ShouldDeleteScreenshot()
	{
		var screenshotsLibrary = PrepareScreenshotsLibrary();
		var selection = PrepareSelectedScreenshot(screenshotsLibrary);
		var command = CreateCommand(selection);
		command.Execute(null);
		screenshotsLibrary.Images.Should().BeEmpty();
	}

	[Fact]
	public void CanExecuteShouldBeTrue()
	{
		var screenshotsLibrary = PrepareScreenshotsLibrary();
		var selection = PrepareSelectedScreenshot(screenshotsLibrary);
		var command = CreateCommand(selection);
		command.CanExecute(null).Should().BeTrue();
	}

	[Fact]
	public void CanExecuteShouldBeFalseWhenNoScreenshot()
	{
		var screenshotsLibrary = PrepareScreenshotsLibrary();
		var selection = PrepareNoSelectedScreenshot(screenshotsLibrary);
		var command = CreateCommand(selection);
		command.CanExecute(null).Should().BeFalse();
	}

	[Fact]
	public void CanExecuteShouldBeFalseWhenScreenshotHasAsset()
	{
		var screenshotsLibrary = PrepareScreenshotsLibrary(out var screenshot);
		MakeAsset(screenshot);
		var selection = PrepareNoSelectedScreenshot(screenshotsLibrary);
		var command = CreateCommand(selection);
		command.CanExecute(null).Should().BeFalse();
	}

	[Fact]
	public void CanExecuteShouldBecomeFalseWhenScreenshotGotAnAsset()
	{
		var screenshotsLibrary = PrepareScreenshotsLibrary(out var screenshot);
		var selection = PrepareSelectedScreenshot(screenshotsLibrary);
		var (observer, annotator) = PrepareAnnotator();
		var command = CreateCommand(selection, annotator);
		MakeAsset(screenshot);
		observer.OnNext(screenshot);
		command.CanExecute(null).Should().BeFalse();
	}

	[Fact]
	public void CanExecuteChangedShouldBeRaisedWhenAssetsChanged()
	{
		var screenshotsLibrary = PrepareScreenshotsLibrary(out var screenshot);
		var selection = PrepareSelectedScreenshot(screenshotsLibrary);
		var (observer, annotator) = PrepareAnnotator();
		var command = CreateCommand(selection, annotator);
		var commandMonitor = command.Monitor();
		MakeAsset(screenshot);
		observer.OnNext(screenshot);
		commandMonitor.Should().Raise(nameof(ICommand.CanExecuteChanged));
	}

	private static ImageSet PrepareScreenshotsLibrary()
	{
		ImageSet imageSet = new();
		imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		return imageSet;
	}

	private static ImageSet PrepareScreenshotsLibrary(out Image image)
	{
		ImageSet imageSet = new();
		image = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		return imageSet;
	}

	private static ScreenshotSelection PrepareSelectedScreenshot(ImageSet imageSet, int screenshotIndex = 0)
	{
		var selectionSubstitute = Substitute.For<ScreenshotSelection>();
		selectionSubstitute.Set.Returns(imageSet);
		selectionSubstitute.SelectedScreenshotIndex.Returns(screenshotIndex);
		selectionSubstitute.SelectedImage.Returns(imageSet.Images[screenshotIndex]);
		return selectionSubstitute;
	}

	private static ScreenshotSelection PrepareNoSelectedScreenshot(ImageSet imageSet)
	{
		var selectionSubstitute = Substitute.For<ScreenshotSelection>();
		selectionSubstitute.Set.Returns(imageSet);
		selectionSubstitute.SelectedScreenshotIndex.Returns(-1);
		return selectionSubstitute;
	}

	private static ICommand CreateCommand(ScreenshotSelection selectionSubstitute, params IReadOnlyCollection<ObservableAnnotator> observableAnnotators)
	{
		var screenshotsDataAccess = PrepareScreenshotsDataAccess();
		DeleteSelectedScreenshotCommandFactory commandFactory = new(selectionSubstitute, observableAnnotators, screenshotsDataAccess);
		var command = commandFactory.CreateCommand();
		return command;
	}

	private static ImageDataAccess PrepareScreenshotsDataAccess()
	{
		var screenshotsDataAccess = Substitute.For<ImageDataAccess>();
		screenshotsDataAccess.When(dataAccess => dataAccess.DeleteImage(Arg.Any<ImageSet>(), Arg.Any<int>())).CallBase();
		return screenshotsDataAccess;
	}

	private static (IObserver<Image> observer, ObservableAnnotator annotator) PrepareAnnotator()
	{
		Subject<Image> screenshotAssetsChanged = new();
		var annotator = Substitute.For<ObservableAnnotator>();
		annotator.AssetsChanged.Returns(screenshotAssetsChanged);
		return (screenshotAssetsChanged.AsObserver(), annotator);
	}

	private static void MakeAsset(Image image)
	{
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(image);
	}
}