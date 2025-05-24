using System.Reactive.Subjects;
using FluentAssertions;
using HotKeys;
using NSubstitute;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Tests.Capturing;

public sealed class HotKeyScreenCapturerTests
{
	[Fact]
	public void ShouldCallImageSaver()
	{
		Subject<Gesture> gestureSubject = new();
		ImageSet imageSet = new();
		Vector2<ushort> imageSize = new(320, 320);
		FakeScreenCapturer<Rgba32> screenCapturer = new();
		FakeImageSaver<Rgba32> imageSaver = new();
		HotKeyScreenCapturer<Rgba32> capturer = new()
		{
			ScreenBoundsProvider = new FakeScreenBoundsProvider(imageSize),
			BindingsManager = new BindingsManager(gestureSubject),
			Set = imageSet,
			FrameRateLimit = null,
			ImageSize = imageSize,
			ScreenCapturer = screenCapturer,
			ImageSaver = imageSaver,
			SelfActivityProvider = Substitute.For<SelfActivityProvider>(),
			ImagesCleaner = new ImagesCleaner(new ImageRepository(Substitute.For<WriteImageDataAccess>()))
		};
		gestureSubject.OnNext(capturer.Gesture);
		Thread.Sleep(100);
		gestureSubject.OnNext(Gesture.Empty);
		screenCapturer.CaptureCalls.Should().NotBeEmpty();
	}
}