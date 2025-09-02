using CommunityToolkit.HighPerformance;
using FluentAssertions;
using SightKeeper.Application.ScreenCapturing;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Tests.Capturing;

public sealed class Bgra32ToRgba32PixelConverterTests
{
	[Fact]
	public void ConvertedPixelsShouldMatchRGBValues()
	{
		Bgra32ToRgba32PixelConverter converter = new();
		Span<Bgra32> source = stackalloc Bgra32[16];
		Random.Shared.NextBytes(source.AsBytes());
		Span<Rgba32> target = stackalloc Rgba32[16];
		var source2D = source.AsSpan2D(4, 4);
		var target2D = target.AsSpan2D(4, 4);
		converter.Convert(source2D, target2D);
		for (int i = 0; i < source.Length; i++)
		{
			var sourcePixel = source[i];
			var targetPixel = target[i];
			targetPixel.R.Should().Be(sourcePixel.R);
			targetPixel.G.Should().Be(sourcePixel.G);
			targetPixel.B.Should().Be(sourcePixel.B);
			// Ignore Alpha channel as I'm not sure if converter should really copy it or just fill with FF's
		}
	}
}