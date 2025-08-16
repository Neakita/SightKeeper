using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class BinaryImageDataSaver<TPixel> : ImageDataSaver<TPixel>
	where TPixel : unmanaged
{
	public void SaveData(Image image, ReadOnlySpan2D<TPixel> data)
	{
		using var stream = image.OpenWriteStream();
		Guard.IsNotNull(stream);
		using var operation = Logger.OperationAt(LogEventLevel.Verbose, LogEventLevel.Error)
			.Begin("Saving image {image} with size {size}", image, image.Size);
		if (TryWriteContiguousSpan(data, stream))
		{
			operation.Complete("contiguous", true);
			return;
		}
		WriteRowByRow(data, stream);
		operation.Complete("contiguous", false);
	}

	private static readonly ILogger Logger = Log.ForContext<BinaryImageDataSaver<TPixel>>();

	private static bool TryWriteContiguousSpan(ReadOnlySpan2D<TPixel> data, Stream stream)
	{
		if (data.TryGetSpan(out var contiguousSpan))
		{
			stream.Write(contiguousSpan.AsBytes());
			return true;
		}
		return false;
	}

	private static void WriteRowByRow(ReadOnlySpan2D<TPixel> data, Stream stream)
	{
		for (int row = 0; row < data.Height; row++)
		{
			var rowSpan = data.GetRowSpan(row);
			stream.Write(rowSpan.AsBytes());
		}
	}
}