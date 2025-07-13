using FlakeId;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;

namespace SightKeeper.Data.Tests.Images;

public sealed class ImageSavingTests
{
	[Fact]
	public void ShouldPersistCreationTimestamp()
	{
		var creationTimestamp = DateTimeOffset.UtcNow.AddDays(-2);
		var set = CreateSetWithImages(creationTimestamp);
		var persistedSet = set.PersistUsingFormatter(SetFormatter);
		var persistedImage = persistedSet.Images.Single();
		persistedImage.CreationTimestamp.Should().Be(creationTimestamp);
	}

	[Fact]
	public void ShouldPersistSize()
	{
		var size = new Vector2<ushort>(480, 320);
		var set = CreateSetWithImage(size);
		var persistedSet = set.PersistUsingFormatter(SetFormatter);
		var persistedImage = persistedSet.Images.Single();
		persistedImage.Size.Should().Be(size);
	}

	[Fact]
	public void ShouldPersistMultipleImages()
	{
		var initialTimestamp = DateTimeOffset.UtcNow;
		var timestamps = Enumerable.Range(0, 5)
			.Select(i => initialTimestamp.AddMilliseconds(i))
			.ToList();
		var set = CreateSetWithImages(timestamps);
		var persistedSet = set.PersistUsingFormatter(SetFormatter);
		persistedSet.Images.Select(image => image.CreationTimestamp).Should().ContainInOrder(timestamps);
	}

	private static ImageSetFormatter SetFormatter => new(new FakeImageSetWrapper(), new InMemoryImageSetFactory(new FakeImageWrapper()), Substitute.For<ImageLookupperPopulator>());

	private static StorableImageSet CreateSetWithImages(params IEnumerable<DateTimeOffset> imageCreationTimestamps)
	{
		var set = Substitute.For<StorableImageSet>();
		var images = imageCreationTimestamps.Select(CreateImage).ToList();
		set.Images.Returns(images);
		return set;
	}

	private static StorableImageSet CreateSetWithImage(Vector2<ushort> imageSize)
	{
		var set = Substitute.For<StorableImageSet>();
		var image = new InMemoryImage(Id.Create(), default, imageSize);
		set.Images.Returns([image]);
		return set;
	}

	private static StorableImage CreateImage(DateTimeOffset creationTimestamp)
	{
		return new InMemoryImage(Id.Create(), creationTimestamp, default);
	}
}