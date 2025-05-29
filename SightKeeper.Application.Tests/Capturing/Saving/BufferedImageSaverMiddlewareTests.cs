using CommunityToolkit.HighPerformance;
using FluentAssertions;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Tests.Capturing.Saving;

public sealed class BufferedImageSaverMiddlewareTests
{
	[Fact]
	public void ShouldHandleLargeBuffer()
	{
		var middleware = CreateMiddleware(out var imageSaver);
		var pixels = CreateRandomPixels(1920, 1080);
		middleware.SaveImage(new ImageSet(), pixels, DateTimeOffset.UtcNow);
		Thread.Sleep(50);
		var receivedPixels = imageSaver.ReceivedCalls.Single().imageData;
		receivedPixels.ShouldRoughlyBe(pixels);
	}

	[Fact]
	public void ShouldHandleVeryLargeBuffer()
	{
		var middleware = CreateMiddleware(out var imageSaver);
		var pixels = CreateRandomPixels(4096, 2160);
		middleware.SaveImage(new ImageSet(), pixels, DateTimeOffset.UtcNow);
		Thread.Sleep(50);
		var receivedPixels = imageSaver.ReceivedCalls.Single().imageData;
		receivedPixels.ShouldRoughlyBe(pixels);
	}

	[Fact]
	public void ShouldBufferImages()
	{
		var middleware = CreateMiddleware(out var imageSaver);
		var pixels = CreateRandomPixels(320, 320);
		imageSaver.HoldCalls = true;
		for (int i = 0; i < 10; i++)
			middleware.SaveImage(new ImageSet(), pixels, DateTimeOffset.UtcNow);
		// first call data dequeues nearly instantly
		middleware.PendingImagesCount.Value.Should().Be(9);
	}

	[Fact]
	public void ShouldFillBuffer()
	{
		var middleware = CreateMiddleware(10, out var imageSaver);
		var pixels = CreateRandomPixels(320, 320);
		imageSaver.HoldCalls = true;
		// first call data will be dequeued even when calls halt
		middleware.SaveImage(new ImageSet(), pixels, DateTimeOffset.UtcNow);
		for (int i = 0; i < 10; i++)
			middleware.SaveImage(new ImageSet(), pixels, DateTimeOffset.UtcNow);
		middleware.IsLimitReached.Should().BeTrue();
	}

	[Fact]
	public void ShouldProcessImagesWhenNextSaverConsumes()
	{
		var middleware = CreateMiddleware(10, out var imageSaver);
		var pixels = CreateRandomPixels(320, 320);
		imageSaver.HoldCalls = true;
		for (int i = 0; i < 10; i++)
			middleware.SaveImage(new ImageSet(), pixels, DateTimeOffset.UtcNow);
		imageSaver.HoldCalls = false;
		Thread.Sleep(100);
		middleware.PendingImagesCount.Value.Should().Be(0);
	}

	[Fact]
	public void ShouldEventuallySendAllDataToNextMiddleware()
	{
		var middleware = CreateMiddleware(10, out var imageSaver);
		var pixels = CreateRandomPixels(320, 320);
		imageSaver.HoldCalls = true;
		for (int i = 0; i < 10; i++)
			middleware.SaveImage(new ImageSet(), pixels, DateTimeOffset.UtcNow);
		imageSaver.HoldCalls = false;
		Thread.Sleep(100);
		imageSaver.ReceivedCalls.Should().HaveCount(10);
	}

	private static BufferedImageSaverMiddleware<Rgba32> CreateMiddleware(out FakeImageSaver<Rgba32> imageSaver)
	{
		imageSaver = new FakeImageSaver<Rgba32>();
		BufferedImageSaverMiddleware<Rgba32> middleware = new()
		{
			NextMiddleware = imageSaver
		};
		return middleware;
	}

	private static Rgba32[,] CreateRandomPixels(int width, int height)
	{
		var pixels = new Rgba32[width, height];
		Random.Shared.NextBytes(pixels.AsSpan().AsBytes());
		return pixels;
	}

	private static BufferedImageSaverMiddleware<Rgba32> CreateMiddleware(ushort maximumAllowedPendingImages, out FakeImageSaver<Rgba32> imageSaver)
	{
		var middleware = CreateMiddleware(out imageSaver);
		middleware.MaximumAllowedPendingImages = maximumAllowedPendingImages;
		return middleware;
	}
}