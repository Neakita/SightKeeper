using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace SightKeeper.Avalonia.Annotation.Images;

internal sealed class DesignAnnotationImageDataContext(
	string sampleImageFileName,
	DateTimeOffset creationTimestamp,
	bool isAsset)
	: AnnotationImageDataContext
{
	public static DesignAnnotationImageDataContext Asset => new("kfSample1.jpg", DateTimeOffset.Now.AddMinutes(3), true);

	public DateTimeOffset CreationTimestamp => creationTimestamp;
	public bool IsAsset => isAsset;

	public Task<Bitmap?> LoadAsync(int? maximumLargestDimension, CancellationToken cancellationToken)
	{
		return _imageDataContext.LoadAsync(maximumLargestDimension, cancellationToken);
	}

	private readonly DesignImageDataContext _imageDataContext = new(sampleImageFileName);
}