using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Screenshots;

public sealed class Screenshot
{
	public DateTimeOffset CreationDate { get; }
	public Vector2<ushort> ImageSize { get; }

	internal Screenshot(DateTimeOffset creationDate, Vector2<ushort> imageSize)
	{
		CreationDate = creationDate;
		Guard.IsGreaterThan<ushort>(imageSize.X, 0);
		Guard.IsGreaterThan<ushort>(imageSize.Y, 0);
		ImageSize = imageSize;
	}
}