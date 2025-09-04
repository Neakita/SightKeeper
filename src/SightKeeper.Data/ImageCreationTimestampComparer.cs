using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

internal sealed class ImageCreationTimestampComparer : IComparer<ImageData>
{
	public static ImageCreationTimestampComparer Instance { get; } = new();

	public int Compare(ImageData? x, ImageData? y)
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