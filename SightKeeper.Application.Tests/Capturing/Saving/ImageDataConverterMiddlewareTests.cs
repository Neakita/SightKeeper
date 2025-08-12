using CommunityToolkit.HighPerformance;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Tests.Capturing.Saving;

public sealed class ImageDataConverterMiddlewareTests
{
	[Fact]
	public void ShouldCallConverter()
	{
		var converterPixels = new Rgba32[4, 4];
		Random.Shared.NextBytes(converterPixels.AsSpan().AsBytes());
		var converter = new FakePixelConverter<Bgra32, Rgba32>(converterPixels);
		var imageSaver = new FakeImageDataSaver<Rgba32>();
		ImageDataConverterMiddleware<Bgra32, Rgba32> middleware = new()
		{
			Converter = converter,
			Next = imageSaver
		};
		var pixels = new Bgra32[4, 4];
		Random.Shared.NextBytes(pixels.AsSpan().AsBytes());
		middleware.SaveData(Substitute.For<Image>(), pixels);
		var receivedPixels = converter.ReceivedCalls.Should().ContainSingle().Subject;
		receivedPixels.Should().BeEquivalentTo(pixels);
	}

	[Fact]
	public void ShouldCallNextMiddleware()
	{
		var converterPixels = new Rgba32[4, 4];
		Random.Shared.NextBytes(converterPixels.AsSpan().AsBytes());
		var converter = new FakePixelConverter<Bgra32, Rgba32>(converterPixels.AsMemory2D());
		var imageSaver = new FakeImageDataSaver<Rgba32>();
		ImageDataConverterMiddleware<Bgra32, Rgba32> middleware = new()
		{
			Converter = converter,
			Next = imageSaver
		};
		var pixels = new Bgra32[4, 4];
		Random.Shared.NextBytes(pixels.AsSpan().AsBytes());
		middleware.SaveData(Substitute.For<Image>(), pixels);
		var receivedPixels = imageSaver.ReceivedCalls.Should().ContainSingle().Subject.data;
		receivedPixels.Should().BeEquivalentTo(converterPixels);
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