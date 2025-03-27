using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace SightKeeper.Avalonia.Annotation.Images;

internal sealed class DesignAnnotationImageDataContext : AnnotationImageDataContext
{
	public DateTimeOffset CreationTimestamp { get; }

	public DesignAnnotationImageDataContext(string sampleImageFileName, DateTimeOffset creationTimestamp)
	{
		_imageDataContext = new DesignImageDataContext(sampleImageFileName);
		CreationTimestamp = creationTimestamp;
	}

	public Task<Bitmap?> Load(int? maximumLargestDimension, CancellationToken cancellationToken)
	{
		return _imageDataContext.Load(maximumLargestDimension, cancellationToken);
	}

	private readonly DesignImageDataContext _imageDataContext;
}