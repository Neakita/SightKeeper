using System.Reactive.Subjects;
using FluentAssertions;
using HotKeys;
using NSubstitute;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Application.Tests.Capturing.Saving;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Tests.Capturing;

public sealed class HotKeyScreenCapturerTests
{
	[Fact]
	public void ShouldCapture()
	{
		var subject = CreateSubject(out var gestureObserver, out var screenCapturer);
		gestureObserver.OnNext(subject.Gesture);
		Thread.Sleep(50);
		gestureObserver.OnNext(Gesture.Empty);
		screenCapturer.CaptureCalls.Should().NotBeEmpty();
	}

	[Fact]
	public void ShouldCaptureCertainNumberOfTimesInTheAllottedTime()
	{
		var subject = CreateSubject(out var gestureObserver, out var screenCapturer);
		subject.FrameRateLimit = 60;
		gestureObserver.OnNext(subject.Gesture);
		Thread.Sleep(1000);
		gestureObserver.OnNext(Gesture.Empty);
		screenCapturer.CaptureCalls.Count.Should().BeInRange(60, 61);
	}

	[Fact]
	public void ShouldCaptureOnce()
	{
		var subject = CreateSubject(out var gestureObserver, out var screenCapturer);
		subject.FrameRateLimit = 0;
		gestureObserver.OnNext(subject.Gesture);
		Thread.Sleep(50);
		gestureObserver.OnNext(Gesture.Empty);
		screenCapturer.CaptureCalls.Count.Should().Be(1);
	}

	[Fact]
	public void ShouldCaptureWithChangedGesture()
	{
		var gesture = new Gesture('A');
		var subject = CreateSubject(out var gestureObserver, out var screenCapturer);
		subject.Gesture = gesture;
		gestureObserver.OnNext(gesture);
		Thread.Sleep(50);
		gestureObserver.OnNext(Gesture.Empty);
		screenCapturer.CaptureCalls.Should().NotBeEmpty();
	}

	[Fact]
	public void ShouldWithdrawFrameRateLimitWhenGestureIsPressed()
	{
		var subject = CreateSubject(out var gestureObserver, out var screenCapturer);
		subject.FrameRateLimit = 0;
		gestureObserver.OnNext(subject.Gesture);
		Thread.Sleep(50);
		subject.FrameRateLimit = null;
		Thread.Sleep(50);
		gestureObserver.OnNext(Gesture.Empty);
		screenCapturer.CaptureCalls.Should().HaveCountGreaterThan(5);
	}

	[Fact]
	public void ShouldNotSetImageSizeToZero()
	{
		var subject = CreateSubject(out _, out _);
		Assert.Throws<ArgumentOutOfRangeException>(() => subject.ImageSize = new Vector2<ushort>(500, 0));
	}

	private static HotKeyScreenCapturer<Rgba32> CreateSubject(out IObserver<Gesture> gestureObserver, out FakeScreenCapturer<Rgba32> screenCapturer)
	{
		Subject<Gesture> gestureSubject = new();
		gestureObserver = gestureSubject;
		screenCapturer = new FakeScreenCapturer<Rgba32>();
		var imageSaver = new FakeImageSaver<Rgba32>();
		HotKeyScreenCapturer<Rgba32> hotKeyScreenCapturer = new()
		{
			ScreenBoundsProvider = new FakeScreenBoundsProvider(new Vector2<ushort>(1920, 1080)),
			BindingsManager = new BindingsManager(gestureSubject),
			Set = Substitute.For<ImageSet>(),
			ScreenCapturer = screenCapturer,
			ImageSaver = imageSaver,
			SelfActivityProvider = Substitute.For<SelfActivityProvider>(),
			ImagesCleaner = new ImagesCleaner(),
		};
		return hotKeyScreenCapturer;
	}
}