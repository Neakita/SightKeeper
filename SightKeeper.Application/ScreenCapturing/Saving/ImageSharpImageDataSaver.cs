using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class ImageSharpImageDataSaver<TPixel>(IImageEncoder encoder) : ImageDataSaver<TPixel> where TPixel : unmanaged, IPixel<TPixel>
{
	public void SaveData(ManagedImage image, ReadOnlySpan2D<TPixel> data)
	{
		using var stream = image.OpenWriteStream();
		Guard.IsNotNull(stream);
		using var operation = Logger.OperationAt(LogEventLevel.Verbose, LogEventLevel.Error)
			.Begin("Saving image {image} with size {size}", image, image.Size);
		if (TrySaveContiguousSpan(data, stream))
		{
			operation.Complete("contiguous", true);
			return;
		}
		SaveWithBuffer(data, stream);
		operation.Complete("contiguous", false);
	}

	private static readonly ILogger Logger = Log.ForContext<ImageSharpImageDataSaver<TPixel>>();
	private TPixel[] _buffer = Array.Empty<TPixel>();

	private unsafe bool TrySaveContiguousSpan(ReadOnlySpan2D<TPixel> data, Stream stream)
	{
		if (!data.TryGetSpan(out var span))
			return false;
		fixed (void* pointer = span)
		{
			var bufferSizeInBytes = span.AsBytes().Length;
			using var imageData = Image.WrapMemory<TPixel>(pointer, bufferSizeInBytes, data.Width, data.Height);
			imageData.Save(stream, encoder);
		}
		return true;
	}

	private void SaveWithBuffer(ReadOnlySpan2D<TPixel> data, Stream stream)
	{
		var requiredBufferLength = data.Width * data.Height;
		if (_buffer.Length < requiredBufferLength)
		{
			_buffer = new TPixel[requiredBufferLength];
		}
		data.CopyTo(_buffer);
		var imageData = Image.WrapMemory<TPixel>(_buffer, data.Width, data.Height);
		imageData.Save(stream, encoder);
	}
}