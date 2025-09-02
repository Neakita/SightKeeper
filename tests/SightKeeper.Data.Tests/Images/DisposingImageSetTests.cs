using NSubstitute;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.ImageSets.Decorators;
using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data.Tests.Images;

public sealed class DisposingImageSetTests
{
	[Fact]
	public void ShouldDisposeImageWhenRemoving()
	{
		var inner = Substitute.For<StorableImageSet>();
		var image = Substitute.For<StorableImage>();
		inner.Images.Returns([image]);
		var set = new DisposingImageSet(inner);
		set.RemoveImageAt(0);
		image.Received().Dispose();
	}

	[Fact]
	public void ShouldDisposeImagesWhenRemovingRange()
	{
		var inner = Substitute.For<StorableImageSet>();
		var image1 = Substitute.For<StorableImage>();
		var image2 = Substitute.For<StorableImage>();
		var image3 = Substitute.For<StorableImage>();
		inner.Images.Returns([image1, image2, image3]);
		var set = new DisposingImageSet(inner);
		set.RemoveImagesRange(1, 2);
		image1.DidNotReceive().Dispose();
		image2.Received().Dispose();
		image3.Received().Dispose();
	}

	[Fact]
	public void ShouldDisposeImagesWhenDisposingSet()
	{
		var inner = Substitute.For<StorableImageSet>();
		var image1 = Substitute.For<StorableImage>();
		var image2 = Substitute.For<StorableImage>();
		inner.Images.Returns([image1, image2]);
		var set = new DisposingImageSet(inner);
		set.Dispose();
		image1.Received().Dispose();
		image2.Received().Dispose();
	}
}