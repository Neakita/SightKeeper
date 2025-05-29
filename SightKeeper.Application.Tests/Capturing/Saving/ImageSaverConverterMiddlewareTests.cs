using CommunityToolkit.HighPerformance;
using FluentAssertions;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Tests.Capturing.Saving;

public sealed class ImageSaverConverterMiddlewareTests
{
	[Fact]
	public void ShouldCallConverter()
	{
		var converter = new FakePixelConverter<Bgra32, Rgba32>();
		var imageSaver = new FakeImageSaver<Rgba32>();
		ImageSaverConverterMiddleware<Bgra32, Rgba32> middleware = new()
		{
			Converter = converter,
			NextMiddleware = imageSaver
		};
		var pixels = new Bgra32[4, 4];
		Random.Shared.NextBytes(pixels.AsSpan().AsBytes());
		middleware.SaveImage(new ImageSet(), pixels, DateTimeOffset.UtcNow);
		converter.ReceivedCalls.Count.Should().Be(1);
		var receivedPixels = converter.ReceivedCalls.Single();
		receivedPixels.ShouldRoughlyBe(pixels);
	}

	[Fact]
	public void ShouldCallNextMiddleware()
	{
		var converter = new Bgra32ToRgba32PixelConverter();
		var imageSaver = new FakeImageSaver<Rgba32>();
		ImageSaverConverterMiddleware<Bgra32, Rgba32> middleware = new()
		{
			Converter = converter,
			NextMiddleware = imageSaver
		};
		var pixels = new Bgra32[4, 4];
		Random.Shared.NextBytes(pixels.AsSpan().AsBytes());
		middleware.SaveImage(new ImageSet(), pixels, DateTimeOffset.UtcNow);
		imageSaver.ReceivedCalls.Should().HaveCount(1);
		var receivedPixels = imageSaver.ReceivedCalls.Single().imageData;
		AssertRoughlyEquals(receivedPixels, pixels);
	}

	private static void AssertRoughlyEquals(Rgba32[,] actualPixels, Bgra32[,] expectedPixels)
	{
		var lengthX = actualPixels.GetLength(0);
		var lengthY = actualPixels.GetLength(1);
		lengthX.Should().Be(expectedPixels.GetLength(0));
		lengthY.Should().Be(expectedPixels.GetLength(1));
		var firstActualPixel = actualPixels[0, 0];
		var firstExpectedPixel = expectedPixels[0, 0];
		AssertMaps(firstActualPixel, firstExpectedPixel);
		var lastActualPixel = actualPixels[lengthX - 1, lengthY - 1];
		var lastExpectedPixel = expectedPixels[lengthX - 1, lengthY - 1];
		AssertMaps(lastActualPixel, lastExpectedPixel);
		for (int i = 0; i < 10; i++)
		{
			var x = Random.Shared.Next(lengthX);
			var y = Random.Shared.Next(lengthY);
			var pixel = actualPixels[x, y];
			var expectedPixel = expectedPixels[x, y];
			AssertMaps(pixel, expectedPixel);
		}
	}

	private static void AssertMaps(Rgba32 pixel, Bgra32 expectedPixel)
	{
		pixel.R.Should().Be(expectedPixel.R);
		pixel.G.Should().Be(expectedPixel.G);
		pixel.B.Should().Be(expectedPixel.B);
	}
}