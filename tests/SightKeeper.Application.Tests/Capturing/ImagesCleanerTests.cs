using System.Collections.ObjectModel;
using CommunityToolkit.Diagnostics;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;
using Range = SightKeeper.Application.Misc.Range;

namespace SightKeeper.Application.Tests.Capturing;

public sealed class ImagesCleanerTests
{
	[Fact]
	public void ShouldCleanImages()
	{
		ImagesCleaner cleaner = new()
		{
			UnusedImagesLimit = 10
		};
		var set = Substitute.For<ImageSet>();
		var images = Enumerable.Range(0, 15).Select(_ =>
		{
			var image = Substitute.For<ManagedImage>();
			image.Assets.Returns(ReadOnlyCollection<Asset>.Empty);
			image.IsInUse.Returns(false);
			return image;
		}).ToList();
		set.Images.Returns(images);
		for (int i = 0; i < 15; i++)
			set.CreateImage(DateTimeOffset.UtcNow.AddMilliseconds(i), new Vector2<ushort>(320, 320));
		cleaner.RemoveExceedUnusedImages(set);
		set.Received().RemoveImagesRange(0, 5);
	}

	[Fact]
	public void ShouldCleanImagesBypassingUsedOnes()
	{
		ImagesCleaner cleaner = new()
		{
			UnusedImagesLimit = 5
		};
		var set = Substitute.For<ImageSet>();
		var images = Enumerable.Range(0, 15).Select(i =>
		{
			var image = Substitute.For<ManagedImage>();
			if (i is 1 or 3)
			{
				image.Assets.Returns([Substitute.For<Asset>()]);
				image.IsInUse.Returns(true);
			}
			else
			{
				image.Assets.Returns(ReadOnlyCollection<Asset>.Empty);
				image.IsInUse.Returns(false);
			}

			return image;
		}).ToList();
		set.Images.Returns(images);
		cleaner.RemoveExceedUnusedImages(set);
		var receivedCalls = set.ReceivedCalls().ToList();
		var actualRemovedRanges = receivedCalls
			.Where(call => call.GetMethodInfo().Name == nameof(ImageSet.RemoveImagesRange)).Select(call =>
			{
				var arguments = call.GetArguments();
				var index = arguments[0];
				Guard.IsNotNull(index);
				var count = arguments[1];
				Guard.IsNotNull(count);
				return Range.FromCount((int)index, (int)count);
			}).ToList();

		//                         0 1 2 3 4 5 6 7 8 9 A B C D F
		//                         H H H H H H H H H H H H H H H
		// these are assets =>       A   A
		// these are reserved =>                       R R R R R
		// so these are deleted => X   X   X X X X X X

		Range[] expectedRemovedRanges =
		[
			Range.FromCount(0, 1),
			Range.FromCount(2, 1),
			Range.FromCount(4, 6)
		];

		// Should remove starting from end, so reverse expected ranges
		actualRemovedRanges.Should().ContainInOrder(expectedRemovedRanges.Reverse());
	}
}