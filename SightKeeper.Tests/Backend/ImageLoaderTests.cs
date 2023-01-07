using FluentAssertions;
using SightKeeper.Backend.Windows;
using SightKeeper.DAL;

namespace SightKeeper.Tests.Backend;

public sealed class ImageLoaderTests
{
	[Fact]
	public void ShouldLoadImageProperly()
	{
		Image image = _imageLoader.GetImageFromFile(DuckImagePath);

		image.Should().NotBeNull();
		image.Content.Length.Should().BeGreaterThan(10000);
	}

	[Fact]
	public void ShouldThrowException()
	{
		_imageLoader.Invoking(loader => loader.GetImageFromFile(LoremIpsumFile)).Should().Throw<Exception>();
	}
	
	
	private readonly ImageLoader _imageLoader = new();

	private const string DuckImagePath = "Backend/Samples/duck.jpg";
	private const string LoremIpsumFile = "Backend/Samples/LoremIpsum.txt";
}