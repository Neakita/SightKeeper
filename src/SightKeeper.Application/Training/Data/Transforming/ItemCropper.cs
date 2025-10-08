using System.Diagnostics.CodeAnalysis;
using SightKeeper.Domain;
using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training.Data.Transforming;

internal interface ItemCropper<TItem>
{
	IEnumerable<TItem> CropItems(IEnumerable<TItem> items, Rectangle cropRectangle, Vector2<ushort> imageSize)
	{
		foreach (var item in items)
			if (TryCrop(item, cropRectangle, imageSize, out var croppedItem))
				yield return croppedItem;
	}

	bool TryCrop(TItem item, Rectangle cropRectangle, Vector2<ushort> imageSize, [MaybeNullWhen(false)] out TItem croppedItem);
}