using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Images;

internal sealed class AnnotationImageViewModel : AnnotationImageDataContext
{
	public DateTimeOffset CreationTimestamp => _image.CreationTimestamp;

	public AnnotationImageViewModel(ImageLoader imageLoader, Image image)
	{
		_imageLoader = imageLoader;
		_image = image;
	}

	public async Task<Bitmap?> Load(int? maximumLargestDimension, CancellationToken cancellationToken)
	{
		return await _imageLoader.LoadImageAsync(_image, maximumLargestDimension, cancellationToken);
	}

	private readonly ImageLoader _imageLoader;
	private readonly Image _image;
}