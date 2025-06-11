using FluentAssertions;
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
		var persistedSet = set.Persist();
		var persistedImage = persistedSet.Images.Single();
		persistedImage.CreationTimestamp.Should().Be(creationTimestamp);
	}

	[Fact]
	public void ShouldPersistSize()
	{
		var size = new Vector2<ushort>(480, 320);
		var set = CreateSetWithImage(size);
		var persistedSet = set.Persist();
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
		var persistedSet = set.Persist();
		persistedSet.Images.Select(image => image.CreationTimestamp).Should().ContainInOrder(timestamps);
	}

	private static ImageSet CreateSetWithImages(params IEnumerable<DateTimeOffset> imageCreationTimestamps)
	{
		var set = Utilities.CreateImageSet();
		foreach (var timestamp in imageCreationTimestamps)
			set.CreateImage(timestamp, new Vector2<ushort>(320, 320));
		return set;
	}

	private static ImageSet CreateSetWithImage(Vector2<ushort> imageSize)
	{
		var set = Utilities.CreateImageSet();
		set.CreateImage(DateTimeOffset.UtcNow, imageSize);
		return set;
	}
}