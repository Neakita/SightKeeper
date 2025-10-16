using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.ImageSets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Images;

public sealed class ImageSetSerializerTests
{
	[Fact]
	public void ShouldCallImagesSerializer()
	{
		var (imageSetSerializer, substituteSerializer) = CreateSerializer();
		var imageSet = Substitute.For<ImageSet>();
		IReadOnlyList<ManagedImage> images = [Substitute.For<ManagedImage>(), Substitute.For<ManagedImage>()];
		imageSet.Images.Returns(images);
		imageSetSerializer.Serialize(imageSet);
		substituteSerializer.Calls.Should().Contain(images);
	}

	private static (ImageSetSerializer, SubstituteSerializer<IReadOnlyCollection<ManagedImage>>) CreateSerializer()
	{
		var imagesSerializer = new SubstituteSerializer<IReadOnlyCollection<ManagedImage>>();
		var imageSetSerializer = new ImageSetSerializer(imagesSerializer);
		return (imageSetSerializer, imagesSerializer);
	}
}