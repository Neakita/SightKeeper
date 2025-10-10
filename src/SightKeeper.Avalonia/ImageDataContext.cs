using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace SightKeeper.Avalonia;

public interface ImageDataContext
{
	Task<Bitmap?> LoadAsync(int? maximumLargestDimension, CancellationToken cancellationToken);
}