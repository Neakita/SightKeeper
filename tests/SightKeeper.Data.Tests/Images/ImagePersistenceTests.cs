using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Images;

public sealed class ImagePersistenceTests
{
	[Fact]
	public void ShouldPersistCreationTimestamp()
	{
		var creationTimestamp = DateTimeOffset.UtcNow.AddDays(-2);
		var image = CreateImage(creationTimestamp);
		var persistedImage = Persist(image);
		persistedImage.CreationTimestamp.Should().Be(creationTimestamp);
	}

	[Fact]
	public void ShouldPersistSize()
	{
		var size = new Vector2<ushort>(123, 456);
		var image = CreateImage(size);
		var persistedImage = Persist(image);
		persistedImage.Size.Should().Be(size);
	}

	private static ManagedImage CreateImage(DateTimeOffset creationTimestamp)
	{
		var image = Substitute.For<ManagedImage, IdHolder>();
		image.CreationTimestamp.Returns(creationTimestamp);
		return image;
	}

	private static ManagedImage CreateImage(Vector2<ushort> size)
	{
		var image = Substitute.For<ManagedImage, IdHolder>();
		image.Size.Returns(size);
		return image;
	}

	private ManagedImage Persist(ManagedImage image)
	{
		var bytes = _imageSerializer.Serialize(image);
		return _imageDeserializer.Deserialize(bytes);
	}

	private readonly ImageSerializer _imageSerializer = new();
	private readonly ImageDeserializer _imageDeserializer = new();
}