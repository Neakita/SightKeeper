using System.Buffers;
using CommunityToolkit.HighPerformance;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Tests.Capturing.Saving;

public sealed class BufferedImageDataSaverMiddlewareTests
{
	[Fact]
	public void ShouldBufferImages()
	{
		var middleware = CreateMiddleware(out var imageSaver);
		var pixels = CreateRandomPixels(320, 320);
		imageSaver.HoldCalls = true;
		var image = Substitute.For<ManagedImage>();
		image.Size.Returns(new Vector2<ushort>(320, 320));
		for (int i = 0; i < 10; i++)
			middleware.SaveData(image, pixels);
		// first call data dequeues nearly instantly
		Thread.Sleep(50);
		middleware.PendingImagesCount.Value.Should().Be(9);
	}

	[Fact]
	public void ShouldFillBuffer()
	{
		var middleware = CreateMiddleware(10, out var imageSaver);
		var pixels = CreateRandomPixels(320, 320);
		imageSaver.HoldCalls = true;
		var image = Substitute.For<ManagedImage>();
		image.Size.Returns(new Vector2<ushort>(320, 320));
		// first call data will be dequeued even when calls halt
		middleware.SaveData(image, pixels);
		// starting process might take some time
		Thread.Sleep(50);
		for (int i = 0; i < 10; i++)
			middleware.SaveData(image, pixels);
		middleware.IsLimitReached.Should().BeTrue();
	}

	[Fact]
	public void ShouldProcessImagesWhenNextSaverConsumes()
	{
		var middleware = CreateMiddleware(10, out var imageSaver);
		var pixels = CreateRandomPixels(320, 320);
		imageSaver.HoldCalls = true;
		var image = Substitute.For<ManagedImage>();
		image.Size.Returns(new Vector2<ushort>(320, 320));
		for (int i = 0; i < 10; i++)
			middleware.SaveData(image, pixels);
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
		var image = Substitute.For<ManagedImage>();
		image.Size.Returns(new Vector2<ushort>(320, 320));
		for (int i = 0; i < 10; i++)
			middleware.SaveData(image, pixels);
		imageSaver.HoldCalls = false;
		Thread.Sleep(100);
		imageSaver.ReceivedCalls.Should().HaveCount(10);
	}

	private static BufferedImageDataSaverMiddleware<Rgba32> CreateMiddleware(out FakeImageDataSaver<Rgba32> imageSaver)
	{
		imageSaver = new FakeImageDataSaver<Rgba32>();
		BufferedImageDataSaverMiddleware<Rgba32> middleware = new()
		{
			Next = imageSaver,
			ArrayPool = ArrayPool<Rgba32>.Create()
		};
		return middleware;
	}

	private static Rgba32[,] CreateRandomPixels(int width, int height)
	{
		var pixels = new Rgba32[width, height];
		Random.Shared.NextBytes(pixels.AsSpan().AsBytes());
		return pixels;
	}

	private static BufferedImageDataSaverMiddleware<Rgba32> CreateMiddleware(ushort maximumAllowedPendingImages, out FakeImageDataSaver<Rgba32> imageSaver)
	{
		var middleware = CreateMiddleware(out imageSaver);
		middleware.MaximumAllowedPendingImages = maximumAllowedPendingImages;
		return middleware;
	}
}