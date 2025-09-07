using System.Collections.ObjectModel;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.Images;

public sealed class DomainImageSetTests
{
	[Fact]
	public void ShouldNotAllowDeleteImageWithAsset()
	{
		var innerSet = Substitute.For<ImageSet>();
		var image = Substitute.For<ManagedImage>();
		image.Assets.Returns([Substitute.For<Asset>()]);
		innerSet.Images.Returns([image]);
		var domainSet = new DomainImageSet(innerSet);
		var exception = Assert.Throws<ImageIsInUseException>(() => domainSet.RemoveImageAt(0));
		exception.Image.Should().Be(image);
		exception.Set.Should().Be(domainSet);
		innerSet.DidNotReceive().RemoveImageAt(0);
	}

	[Fact]
	public void ShouldAllowDeleteImageWithoutAssets()
	{
		var innerSet = Substitute.For<ImageSet>();
		var image = Substitute.For<ManagedImage>();
		image.Assets.Returns(ReadOnlyCollection<Asset>.Empty);
		innerSet.Images.Returns([image]);
		var domainSet = new DomainImageSet(innerSet);
		domainSet.RemoveImageAt(0);
		innerSet.Received().RemoveImageAt(0);
	}

	[Fact]
	public void ShouldNotCreateImageWithZeroSizeDimensions()
	{
		var innerSet = Substitute.For<ImageSet>();
		DomainImageSet domainSet = new(innerSet);
		Assert.Throws<ArgumentException>(() => domainSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(0, 320)));
		innerSet.DidNotReceive().CreateImage(Arg.Any<DateTimeOffset>(), Arg.Any<Vector2<ushort>>());
	}

	[Fact]
	public void ShouldCreateImage()
	{
		var innerSet = Substitute.For<ImageSet>();
		var domainSet = new DomainImageSet(innerSet);
		var creationTimestamp = DateTimeOffset.Now;
		var size = new Vector2<ushort>(480, 480);
		var expectedImage = Substitute.For<ManagedImage>();
		innerSet.CreateImage(creationTimestamp, size).Returns(expectedImage);
		var image = domainSet.CreateImage(creationTimestamp, size);
		image.Should().Be(expectedImage);
	}

	[Fact]
	public void ShouldNotAllowCreateImageWithEarlierTimestampThanLatestCreated()
	{
		var earlierTimestamp = DateTimeOffset.UtcNow;
		var laterTimestamp = earlierTimestamp.AddHours(2);
		var innerSet = Substitute.For<ImageSet>();
		var laterImage = Substitute.For<ManagedImage>();
		laterImage.CreationTimestamp.Returns(laterTimestamp);
		innerSet.Images.Returns([laterImage]);
		var domainSet = new DomainImageSet(innerSet);
		var exception = Assert.Throws<InconsistentImageCreationTimestampException>(() =>
			domainSet.CreateImage(earlierTimestamp, new Vector2<ushort>(320, 320)));
		exception.AffectedSet.Should().Be(domainSet);
		exception.NewImageCreationTimestamp.Should().Be(earlierTimestamp);
		innerSet.DidNotReceive().CreateImage(Arg.Any<DateTimeOffset>(), Arg.Any<Vector2<ushort>>());
	}

	[Fact]
	public void ShouldGetImagesRange()
	{
		var innerSet = Substitute.For<ImageSet>();
		IReadOnlyList<ManagedImage> expectedImages = [Substitute.For<ManagedImage>(), Substitute.For<ManagedImage>()];
		innerSet.GetImagesRange(1, 2).Returns(expectedImages);
		var domainSet = new DomainImageSet(innerSet);
		var images = domainSet.GetImagesRange(1, 2);
		images.Should().BeSameAs(expectedImages);
		innerSet.Received().GetImagesRange(1, 2);
	}

	[Fact]
	public void ShouldRemoveImagesRange()
	{
		var innerSet = Substitute.For<ImageSet>();
		var domainSet = new DomainImageSet(innerSet);
		domainSet.RemoveImagesRange(2, 4);
		innerSet.Received().RemoveImagesRange(2, 4);
	}
}