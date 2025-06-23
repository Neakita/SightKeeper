using FlakeId;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Saving;

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

	private static ImageSetFormatter SetFormatter => new(new FakeImageSetWrapper(), new FakeImageWrapper());

	private static ImageSet CreateSetWithImages(params IEnumerable<DateTimeOffset> imageCreationTimestamps)
	{
		var set = Substitute.For<ImageSet>();
		var images = imageCreationTimestamps.Select(CreateImage).ToList();
		set.Images.Returns(images);
		return set;
	}

	private static ImageSet CreateSetWithImage(Vector2<ushort> imageSize)
	{
		var set = Substitute.For<ImageSet>();
		var image = new InMemoryImage(Id.Create(), default, imageSize);
		set.Images.Returns([image]);
		return set;
	}

	private static Image CreateImage(DateTimeOffset creationTimestamp)
	{
		return new InMemoryImage(Id.Create(), creationTimestamp, default);
	}
}