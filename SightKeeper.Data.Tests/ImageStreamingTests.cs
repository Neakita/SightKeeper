using FluentAssertions;
using SightKeeper.Domain;

namespace SightKeeper.Data.Tests;

public sealed class ImageStreamingTests
{
	[Fact]
	public void ShouldWriteAndReadStream()
	{
		var set = Utilities.CreateImageSet();
		var image = set.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		using (var writeStream = image.OpenWriteStream())
		{
			writeStream.Should().NotBeNull();
			writeStream.Write([1, 2, 3]);
		}
		using (var readStream = image.OpenReadStream())
		{
			readStream.Should().NotBeNull();
			var buffer = new byte[3];
			var bytesRead = readStream.Read(buffer);
			bytesRead.Should().Be(3);
			buffer.Should().ContainInOrder(1, 2, 3);
		}
	}
}