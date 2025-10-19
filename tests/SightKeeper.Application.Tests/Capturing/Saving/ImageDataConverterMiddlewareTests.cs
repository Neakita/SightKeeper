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
		var imageSaver = new FakeImageDataSaver<Rgba32>();
		var converter = new FakePixelConverter<Bgra32, Rgba32>(converterPixels);
		ImageDataConverterMiddleware<Bgra32, Rgba32> middleware = new(imageSaver, converter);
		var pixels = new Bgra32[4, 4];
		Random.Shared.NextBytes(pixels.AsSpan().AsBytes());
		middleware.SaveData(Substitute.For<ManagedImage>(), pixels);
		var receivedPixels = converter.ReceivedCalls.Should().ContainSingle().Subject;
		receivedPixels.Should().BeEquivalentTo(pixels);
	}

	[Fact]
	public void ShouldCallNextMiddleware()
	{
		var converterPixels = new Rgba32[4, 4];
		Random.Shared.NextBytes(converterPixels.AsSpan().AsBytes());
		var imageSaver = new FakeImageDataSaver<Rgba32>();
		var converter = new FakePixelConverter<Bgra32, Rgba32>(converterPixels.AsMemory2D());
		ImageDataConverterMiddleware<Bgra32, Rgba32> middleware = new(imageSaver, converter);
		var pixels = new Bgra32[4, 4];
		Random.Shared.NextBytes(pixels.AsSpan().AsBytes());
		middleware.SaveData(Substitute.For<ManagedImage>(), pixels);
		var receivedPixels = imageSaver.ReceivedCalls.Should().ContainSingle().Subject.data;
		receivedPixels.Should().BeEquivalentTo(converterPixels);
	}
}