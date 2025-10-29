using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

internal sealed class BinaryImageDataSaver<TPixel>(ILogger logger) : ImageDataSaver<TPixel>
	where TPixel : unmanaged
{
	public void SaveData(ManagedImage image, ReadOnlySpan2D<TPixel> data)
	{
		var streamableData = image.GetFirst<StreamableData>();
		using var stream = streamableData.OpenWriteStream();
		Guard.IsNotNull(stream);
		using var operation = logger.OperationAt(LogEventLevel.Verbose, LogEventLevel.Error)
			.Begin("Saving image {image} with size {size}", image, image.Size);
		if (TryWriteContiguousSpan(data, stream))
		{
			operation.Complete("contiguous", true);
			return;
		}
		WriteRowByRow(data, stream);
		operation.Complete("contiguous", false);
	}

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