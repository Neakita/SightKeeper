using System.Reactive;
using System.Reactive.Subjects;
using System.Windows.Input;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.Annotation;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.Tooling.Commands;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Screenshots;

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
		screenshotsLibrary.Screenshots.Should().BeEmpty();
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

	private static ScreenshotsLibrary PrepareScreenshotsLibrary()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		return screenshotsLibrary;
	}

	private static ScreenshotsLibrary PrepareScreenshotsLibrary(out Screenshot screenshot)
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		return screenshotsLibrary;
	}

	private static ScreenshotSelection PrepareSelectedScreenshot(ScreenshotsLibrary screenshotsLibrary, int screenshotIndex = 0)
	{
		var selectionSubstitute = Substitute.For<ScreenshotSelection>();
		selectionSubstitute.Library.Returns(screenshotsLibrary);
		selectionSubstitute.SelectedScreenshotIndex.Returns(screenshotIndex);
		selectionSubstitute.SelectedScreenshot.Returns(screenshotsLibrary.Screenshots[screenshotIndex]);
		return selectionSubstitute;
	}

	private static ScreenshotSelection PrepareNoSelectedScreenshot(ScreenshotsLibrary screenshotsLibrary)
	{
		var selectionSubstitute = Substitute.For<ScreenshotSelection>();
		selectionSubstitute.Library.Returns(screenshotsLibrary);
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

	private static ScreenshotsDataAccess PrepareScreenshotsDataAccess()
	{
		var screenshotsDataAccess = Substitute.For<ScreenshotsDataAccess>();
		screenshotsDataAccess.When(dataAccess => dataAccess.DeleteScreenshot(Arg.Any<ScreenshotsLibrary>(), Arg.Any<int>())).CallBase();
		return screenshotsDataAccess;
	}

	private static (IObserver<Screenshot> observer, ObservableAnnotator annotator) PrepareAnnotator()
	{
		Subject<Screenshot> screenshotAssetsChanged = new();
		var annotator = Substitute.For<ObservableAnnotator>();
		annotator.AssetsChanged.Returns(screenshotAssetsChanged);
		return (screenshotAssetsChanged.AsObserver(), annotator);
	}

	private static void MakeAsset(Screenshot screenshot)
	{
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(screenshot);
	}
}