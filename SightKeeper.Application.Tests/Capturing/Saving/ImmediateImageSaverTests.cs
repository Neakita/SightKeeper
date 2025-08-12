using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Tests.Capturing.Saving;

public sealed class ImmediateImageSaverTests
{
	[Fact]
	public void ShouldCreateImageAndDelegateDataSaving()
	{
		var dataSaver = new FakeImageDataSaver<Argb32>();
		ImmediateImageSaver<Argb32> saver = new()
		{
			DataSaver = dataSaver
		};
		var set = Substitute.For<ImageSet>();
		var image = Substitute.For<Image>();
		var data = new Argb32[2, 2];
		data[0, 0] = new Argb32(0x1, 0x2, 0x3);
		data[0, 1] = new Argb32(0x4, 0x5, 0x6);
		data[1, 0] = new Argb32(0x7, 0x8, 0x9);
		data[1, 1] = new Argb32(0xA, 0xB, 0xC);
		set.CreateImage(Arg.Any<DateTimeOffset>(), Arg.Any<Vector2<ushort>>()).Returns(image);
		saver.SaveImage(set, data, DateTimeOffset.UtcNow);
		set.Received().CreateImage(Arg.Any<DateTimeOffset>(), Arg.Any<Vector2<ushort>>());
		var (receivedImage, receivedData) = dataSaver.ReceivedCalls.Should().ContainSingle().Subject;
		receivedImage.Should().Be(image);
		receivedData.Should().BeEquivalentTo(data);
	}
}