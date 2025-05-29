using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests.Capturing;

public sealed class ImagesCleanerTests
{
	[Fact]
	public void ShouldCleanImages()
	{
		ImagesCleaner cleaner = new(new ImageRepository(Substitute.For<WriteImageDataAccess>()))
		{
			UnusedImagesLimit = 10
		};
		ImageSet set = new();
		for (int i = 0; i < 15; i++)
			set.CreateImage(DateTimeOffset.UtcNow.AddMilliseconds(i), new Vector2<ushort>(320, 320));
		var expectedRemovedImages = set.Images.Take(5).ToList();
		cleaner.RemoveExceedUnusedImages(set);
		set.Images.Should().NotContain(expectedRemovedImages);
	}

	[Fact]
	public void ShouldCleanImagesBypassingUsedOnes()
	{
		ImagesCleaner cleaner = new(new ImageRepository(Substitute.For<WriteImageDataAccess>()))
		{
			UnusedImagesLimit = 10
		};
		ImageSet set = new();
		for (int i = 0; i < 15; i++)
			set.CreateImage(DateTimeOffset.UtcNow.AddMilliseconds(i), new Vector2<ushort>(320, 320));
		var dataSet = new DetectorDataSet();
		dataSet.AssetsLibrary.MakeAsset(set.Images[1]);
		dataSet.AssetsLibrary.MakeAsset(set.Images[3]);
		var usedImages = set.Images.Where(image => image.IsInUse).ToList();
		var expectedRemovedImages = set.Images.Where(image => !image.IsInUse).Take(3).ToList();
		cleaner.RemoveExceedUnusedImages(set);
		set.Images.Should().NotContain(expectedRemovedImages);
		set.Images.Should().Contain(usedImages);
		// 2 with assets and 10 more reserved
		set.Images.Count.Should().Be(12);
	}
}