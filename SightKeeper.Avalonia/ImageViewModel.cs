using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using SightKeeper.Avalonia.Annotation.Images;

namespace SightKeeper.Avalonia;

internal sealed class ImageViewModel : ImageDataContext
{
	public ImageViewModel(ImageLoader imageLoader, DomainImage image)
	{
		_imageLoader = imageLoader;
		_image = image;
	}

	public async Task<Bitmap?> Load(int? maximumLargestDimension, CancellationToken cancellationToken)
	{
		return await _imageLoader.LoadImageAsync(_image, maximumLargestDimension, cancellationToken);
	}

	private readonly ImageLoader _imageLoader;
	private readonly DomainImage _image;
}