using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public sealed class ImageDataWriter<TPixel> : ImageDataSaver<TPixel>
	where TPixel : unmanaged
{
	public void SaveData(Image image, ReadOnlySpan2D<TPixel> data)
	{
		using var stream = image.OpenWriteStream();
		Guard.IsNotNull(stream);
		if (TryWriteContiguousSpan(data, stream))
			return;
		WriteRowByRow(data, stream);
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