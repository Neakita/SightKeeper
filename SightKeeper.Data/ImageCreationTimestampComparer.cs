using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

internal sealed class ImageCreationTimestampComparer<TImage> : IComparer<TImage> where TImage : Image
{
	public static ImageCreationTimestampComparer<TImage> Instance { get; } = new();

	public int Compare(TImage? x, TImage? y)
	{
		if (ReferenceEquals(x, y))
			return 0;
		if (y is null)
			return 1;
		if (x is null)
			return -1;
		return x.CreationTimestamp.CompareTo(y.CreationTimestamp);
	}
}