using System.Diagnostics.CodeAnalysis;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training.Data.Transforming;

public sealed class AssetItemCropper : ItemCropper<AssetItemData>
{
	public bool TryCrop(AssetItemData item, Rectangle cropRectangle, Vector2<ushort> imageSize, [MaybeNullWhen(false)] out AssetItemData croppedItem)
	{
		if (TryCrop(item.Bounding, cropRectangle, imageSize, out var croppedBounding))
		{
			croppedItem = new AssetItemDataValue(item.Tag, croppedBounding);
			return true;
		}
		croppedItem = null;
		return false;
	}

	private static bool TryCrop(Bounding itemBounding, Rectangle cropRectangle, Vector2<ushort> imageSize, out Bounding croppedBounding)
	{
		croppedBounding = default;
		var sizedItemBounding = new Bounding(
			itemBounding.Left * imageSize.X,
			itemBounding.Top * imageSize.Y,
			itemBounding.Right * imageSize.X,
			itemBounding.Bottom * imageSize.Y);
		var sizedTopLeft = sizedItemBounding.TopLeft - new Vector2<double>(cropRectangle.Left, cropRectangle.Top);
		var sizedBottomRight = sizedItemBounding.BottomRight - new Vector2<double>(cropRectangle.Left, cropRectangle.Top);

		var targetWidth = cropRectangle.Width;
		var targetHeight = cropRectangle.Height;

		var left = sizedTopLeft.X / targetWidth;
		var top = sizedTopLeft.Y / targetHeight;
		var right = sizedBottomRight.X / targetWidth;
		var bottom = sizedBottomRight.Y / targetHeight;

		if (left > 1 || top > 1 || right < 0 || bottom < 0)
			return false;

		left = Math.Clamp(left, 0, 1);
		top = Math.Clamp(top, 0, 1);
		right = Math.Clamp(right, 0, 1);
		bottom = Math.Clamp(bottom, 0, 1);
		
		croppedBounding = new Bounding(left, top, right, bottom);

		return true;
	}

	private static Vector2<double> Clamp(Vector2<double> value, Vector2<double> minimum, Vector2<double> maximum)
	{
		return new Vector2<double>(
			Math.Clamp(value.X, minimum.X, maximum.X),
			Math.Clamp(value.Y, minimum.Y, maximum.Y));
	}
}