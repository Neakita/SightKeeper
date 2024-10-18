using System.Buffers;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Application.Screenshotting.Saving;

internal sealed class ScreenshotData<TPixel> : IDisposable
	where TPixel : unmanaged
{
	public ScreenshotsLibrary Library { get; }
	public DateTimeOffset CreationDate { get; }
	public Vector2<ushort> ImageSize { get; }
	public ReadOnlySpan<TPixel> ImageData => _imageData.AsSpan()[.._imageDataLength];

	public ScreenshotData(ScreenshotsLibrary library, DateTimeOffset creationDate, ReadOnlySpan2D<TPixel> imageData)
	{
		Library = library;
		CreationDate = creationDate;
		ImageSize = new Vector2<ushort>((ushort)imageData.Width, (ushort)imageData.Height);
		_imageDataLength = imageData.Width * imageData.Height;
		_imageData = ArrayPool<TPixel>.Shared.Rent(_imageDataLength);
		imageData.CopyTo(_imageData);
	}

	private readonly int _imageDataLength;
	private readonly TPixel[] _imageData;

	public void Dispose()
	{
		ArrayPool<TPixel>.Shared.Return(_imageData);
	}
}