using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.Misc;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Images;

public sealed class ImageSetDeserializerTests
{
	[Fact]
	public void ShouldCallImagesDeserializer()
	{
		var (imageSetDeserializer, imagesDeserializer) = CreateDeserializer();
		var bytes = SerializeDefaultImageSet();
		imageSetDeserializer.Deserialize(bytes);
		imagesDeserializer.CallsCounts.Should().Be(1);
	}

	[Fact]
	public void ShouldAddImagesFromImagesDeserializerToSet()
	{
		IReadOnlyCollection<ManagedImage> images = [Substitute.For<ManagedImage>(), Substitute.For<ManagedImage>()];
		var (imageSetDeserializer, _) = CreateDeserializer(images);
		var bytes = SerializeDefaultImageSet();
		var imageSet = imageSetDeserializer.Deserialize(bytes);
		var settableInitialImages = (SettableInitialItems<ManagedImage>)imageSet;
		foreach (var image in images)
			settableInitialImages.Received().WrapAndInsert(image);
	}

	[Fact]
	public void ShouldEnsureImageSetCapacity()
	{
		IReadOnlyCollection<ManagedImage> images = [Substitute.For<ManagedImage>(), Substitute.For<ManagedImage>()];
		var (imageSetDeserializer, _) = CreateDeserializer(images);
		var bytes = SerializeDefaultImageSet();
		var imageSet = imageSetDeserializer.Deserialize(bytes);
		var settableInitialImages = (SettableInitialItems<ManagedImage>)imageSet;
		settableInitialImages.Received().EnsureCapacity(images.Count);
	}

	private static (ImageSetDeserializer, SubstituteDeserializer<IReadOnlyCollection<ManagedImage>>) CreateDeserializer()
	{
		return CreateDeserializer([]);
	}

	private static (ImageSetDeserializer, SubstituteDeserializer<IReadOnlyCollection<ManagedImage>>) CreateDeserializer(IReadOnlyCollection<ManagedImage> images)
	{
		var imageSetFactory = Substitute.For<Factory<ImageSet>>();
		imageSetFactory.Create().Returns(_ => Substitute.For<ImageSet, SettableInitialItems<ManagedImage>>());
		var imagesDeserializer = new SubstituteDeserializer<IReadOnlyCollection<ManagedImage>>(() => images);
		var imageSetDeserializer = new ImageSetDeserializer(imageSetFactory, imagesDeserializer);
		return (imageSetDeserializer, imagesDeserializer);
	}

	private static byte[] SerializeDefaultImageSet()
	{
		var imageSet = Substitute.For<ImageSet>();
		return Serialize(imageSet);
	}

	private static byte[] Serialize(ImageSet imageSet)
	{
		var imagesSerializer = new SubstituteSerializer<IReadOnlyCollection<ManagedImage>>();
		var imageSetSerializer = new ImageSetSerializer(imagesSerializer);
		return imageSetSerializer.Serialize(imageSet);
	}
}