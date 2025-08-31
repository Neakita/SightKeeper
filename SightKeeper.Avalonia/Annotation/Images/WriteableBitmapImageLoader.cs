using System.Threading;
using System.Threading.Tasks;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Images;

public interface WriteableBitmapImageLoader
{
	Task<PooledWriteableBitmap?> LoadImageAsync(
		ManagedImage image,
		int? maximumLargestDimension,
		CancellationToken cancellationToken);
}