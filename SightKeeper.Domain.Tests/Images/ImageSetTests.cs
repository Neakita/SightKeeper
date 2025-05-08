using FluentAssertions;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.Images;

public sealed class ImageSetTests
{
	[Fact]
	public void ShouldNotDeleteImageWithAsset()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(image);
		var exception = Assert.Throws<ImageIsInUseException>(() => imageSet.RemoveImageAt(0));
		exception.Image.Should().Be(image);
		exception.Set.Should().Be(imageSet);
	}

	[Fact]
	public void ShouldDeleteImageAfterAssetDeletion()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(image);
		dataSet.AssetsLibrary.DeleteAsset(image);
		imageSet.RemoveImageAt(0);
	}

	[Fact]
	public void ShouldDeleteImageAfterAssetsLibraryClear()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(image);
		dataSet.AssetsLibrary.ClearAssets();
		imageSet.RemoveImageAt(0);
	}

	[Fact]
	public void ShouldNotCreateImageWithAnyZeroSizeDimensions()
	{
		ImageSet set = new();
		Assert.Throws<ArgumentException>(() => set.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(0, 320)));
	}

	[Fact]
	public void ShouldCreateImage()
	{
		var set = new ImageSet();
		var creationTimestamp = DateTimeOffset.Now;
		var size = new Vector2<ushort>(480, 480);
		var image = set.CreateImage(creationTimestamp, size);
		image.Set.Should().Be(set);
		image.CreationTimestamp.Should().Be(creationTimestamp);
		image.Size.Should().Be(size);
		set.Images.Should().Contain(image);
	}

	[Fact]
	public void ShouldNotCreateImageWithEarlierTimestampThanLatestCreated()
	{
		var set = new ImageSet();
		var earlierTimestamp = DateTimeOffset.Now;
		var laterTimestamp = earlierTimestamp.AddHours(2);
		set.CreateImage(laterTimestamp, new Vector2<ushort>(320, 320));
		var exception = Assert.Throws<InconsistentImageCreationTimestampException>(() =>
			set.CreateImage(earlierTimestamp, new Vector2<ushort>(320, 320)));
		exception.AffectedSet.Should().Be(set);
		exception.NewImageCreationTimestamp.Should().Be(earlierTimestamp);
	}

	[Fact]
	public void ShouldGetImagesRange()
	{
		var set = new ImageSet();
		var initialTimestamp = DateTimeOffset.UtcNow;
		for (int i = 0; i < 10; i++)
			set.CreateImage(initialTimestamp.AddSeconds(i), new Vector2<ushort>(320, 320));
		var images = set.GetImagesRange(2, 4);
		var timestamps = images.Select(image => image.CreationTimestamp);
		var expectedTimestamps = Enumerable.Range(2, 4).Select(offset => initialTimestamp.AddSeconds(offset));
		timestamps.Should().ContainInOrder(expectedTimestamps);
	}

	[Fact]
	public void ShouldRemoveImagesRange()
	{
		var set = new ImageSet();
		var initialTimestamp = DateTimeOffset.UtcNow;
		for (int i = 0; i < 10; i++)
			set.CreateImage(initialTimestamp.AddSeconds(i), new Vector2<ushort>(320, 320));
		set.RemoveImagesRange(2, 4);
		var removedImagesTimestamps = Enumerable.Range(2, 4).Select(offset => initialTimestamp.AddSeconds(offset));
		set.Images.Should().NotContain(image => removedImagesTimestamps.Contains(image.CreationTimestamp));
	}
}