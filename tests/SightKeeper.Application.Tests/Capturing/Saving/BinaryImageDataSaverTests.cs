using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Tests.Capturing.Saving;

public sealed class BinaryImageDataSaverTests
{
	[Fact]
	public void ShouldWriteDataToStream()
	{
		var image = Substitute.For<ManagedImage>();
		using MemoryStream stream = new();
		image.Size.Returns(new Vector2<ushort>(2, 2));
		image.OpenWriteStream().Returns(stream);
		BinaryImageDataSaver<Argb32> dataSaver = new();
		var pixels =  new Argb32[2, 2];
		pixels[0, 0] = new Argb32(0x1, 0x2, 0x3);
		pixels[0, 1] = new Argb32(0x4, 0x5, 0x6);
		pixels[1, 0] = new Argb32(0x7, 0x8, 0x9);
		pixels[1, 1] = new Argb32(0xA, 0xB, 0xC);
		dataSaver.SaveData(image, pixels);
		var receivedPixels = stream.ToArray();
		receivedPixels.Should().ContainInOrder(
			0xFF, 0x1, 0x2, 0x3,
			0xFF, 0x4, 0x5, 0x6,
			0xFF, 0x7, 0x8, 0x9,
			0xFF, 0xA, 0xB, 0xC);
	}
}