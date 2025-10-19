using NSubstitute;
using SightKeeper.Data.ImageSets.Decorators;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Images;

public sealed class DisposingImageSetTests
{
	[Fact]
	public void ShouldDisposeImageWhenRemoving()
	{
		var inner = Substitute.For<ImageSet>();
		var image = Substitute.For<ManagedImage, IDisposable>();
		inner.Images.Returns([image]);
		var set = new DisposingImageSet(inner);
		set.RemoveImageAt(0);
		var disposableImage = (IDisposable)image;
		disposableImage.Received().Dispose();
	}

	[Fact]
	public void ShouldDisposeImagesWhenRemovingRange()
	{
		var inner = Substitute.For<ImageSet>();
		var image1 = Substitute.For<ManagedImage, IDisposable>();
		var image2 = Substitute.For<ManagedImage, IDisposable>();
		var image3 = Substitute.For<ManagedImage, IDisposable>();
		inner.Images.Returns([image1, image2, image3]);
		var set = new DisposingImageSet(inner);
		set.RemoveImagesRange(1, 2);
		((IDisposable)image1).DidNotReceive().Dispose();
		((IDisposable)image2).Received().Dispose();
		((IDisposable)image3).Received().Dispose();
	}

	[Fact]
	public void ShouldDisposeImagesWhenDisposingSet()
	{
		var inner = Substitute.For<ImageSet>();
		var image1 = Substitute.For<ManagedImage, IDisposable>();
		var image2 = Substitute.For<ManagedImage, IDisposable>();
		inner.Images.Returns([image1, image2]);
		var set = new DisposingImageSet(inner);
		set.Dispose();
		((IDisposable)image1).Received().Dispose();
		((IDisposable)image2).Received().Dispose();
	}
}