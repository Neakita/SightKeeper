using CommunityToolkit.Diagnostics;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training.Data.Transforming;

internal sealed class RandomItemsCropRectanglesProvider<TAsset, TItem>(RandomItemsCropSettings settings) : CropRectanglesProvider<TAsset>
	where TAsset : ReadOnlyItemsAsset<TItem>
	where TItem : ReadOnlyDetectorItem
{
	public IEnumerable<Rectangle> GetCropRectangles(TAsset asset)
	{
		var imageSize = asset.Image.Size;
		if (imageSize.X < settings.TargetSize.X || imageSize.Y < settings.TargetSize.Y)
			yield break;
		if (imageSize == settings.TargetSize)
		{
			yield return new Rectangle(0, 0, imageSize.X, imageSize.Y);
			yield break;
		}
		var existingRectangles = new List<Rectangle>();
		for (int i = 0; i < settings.MaximumSamplesPerItem; i++)
			foreach (var item in asset.Items)
			{
				var rectangle = GetCropRectangle(item, imageSize);
				if (existingRectangles.Any(existingRectangle => GetIoU(rectangle, existingRectangle) > settings.IoUThreshold))
					continue;
				existingRectangles.Add(rectangle);
				yield return rectangle;
			}
	}

	private Rectangle GetCropRectangle(TItem item, Vector2<ushort> imageSize)
	{
		var sizedItemBounding = item.Bounding * imageSize.ToDouble();
		var cropRectanglePosition = sizedItemBounding.Center - settings.TargetSize.ToDouble() / 2;
		cropRectanglePosition += GetRandomOffset(sizedItemBounding, imageSize);
		var roundedCropRectanglePosition = cropRectanglePosition.ToUInt16();
		var maximumCropRectanglePosition = imageSize - settings.TargetSize;
		Guard.IsBetweenOrEqualTo<ushort>(roundedCropRectanglePosition.X, 0, maximumCropRectanglePosition.X);
		Guard.IsBetweenOrEqualTo<ushort>(roundedCropRectanglePosition.Y, 0, maximumCropRectanglePosition.Y);
		return new Rectangle(
			roundedCropRectanglePosition.X,
			roundedCropRectanglePosition.Y,
			settings.TargetSize.X,
			settings.TargetSize.Y);
	}

	private Vector2<double> GetRandomOffset(Bounding sizedItemBounding, Vector2<ushort> imageSize)
	{
		var (minimumOffset, maximumOffset) = GetOffsetConstraints(sizedItemBounding, imageSize);
		return new Vector2<double>(
			GetRandomDouble(minimumOffset.X, maximumOffset.X),
			GetRandomDouble(minimumOffset.Y, maximumOffset.Y));
	}

	private (Vector2<double> minimumOffset, Vector2<double> maximumOffset) GetOffsetConstraints(Bounding sizedItemBounding, Vector2<ushort> imageSize)
	{
		var targetSize = settings.TargetSize.ToDouble();
		var inItemMaximumOffset = (targetSize - sizedItemBounding.Size) / 2;
		inItemMaximumOffset = Max(Vector2<double>.Zero, inItemMaximumOffset);

		var aroundItemMaximumOffset = (sizedItemBounding.Size - targetSize) / 2;
		aroundItemMaximumOffset = Max(Vector2<double>.Zero, aroundItemMaximumOffset);

		var outItemMaximumOffset = sizedItemBounding.Size / 2 + targetSize / 2;

		var absoluteMinimumOffset = -(sizedItemBounding.Center - targetSize / 2);
		var absoluteMaximumOffset = imageSize.ToDouble() - (sizedItemBounding.Center + targetSize / 2);

		var combinedMaximumOffset = Vector2<double>.Zero;
		combinedMaximumOffset += inItemMaximumOffset * settings.MaximumInItemOffset;
		combinedMaximumOffset += aroundItemMaximumOffset;
		combinedMaximumOffset += (outItemMaximumOffset - inItemMaximumOffset - aroundItemMaximumOffset) * settings.MaximumOutItemOffset;

		var combinedMinimumOffset = -combinedMaximumOffset;

		combinedMinimumOffset = Max(combinedMinimumOffset, absoluteMinimumOffset);
		combinedMaximumOffset = Min(combinedMaximumOffset, absoluteMaximumOffset);
		return (combinedMinimumOffset, combinedMaximumOffset);
	}

	private static Vector2<double> Min(Vector2<double> a, Vector2<double> b)
	{
		return new Vector2<double>(
			Math.Min(a.X, b.X),
			Math.Min(a.Y, b.Y));
	}

	private static Vector2<double> Max(Vector2<double> a, Vector2<double> b)
	{
		return new Vector2<double>(
			Math.Max(a.X, b.X),
			Math.Max(a.Y, b.Y));
	}

	private static bool TryGetIntersection(Rectangle a, Rectangle b, out Rectangle intersection)
	{
		var left = Math.Max(a.Left, b.Left);
		var top = Math.Max(a.Top, b.Top);
		var right = Math.Min(a.Right, b.Right);
		var bottom = Math.Min(a.Bottom, b.Bottom);
		if (right > left && bottom > top)
		{
			intersection = Rectangle.FromLTRB(left, top, right, bottom);
			return true;
		}
		intersection = default;
		return false;
	}

	private static double GetIoU(Rectangle a, Rectangle b)
	{
		if (!TryGetIntersection(a, b, out var intersection))
			return 0;
		var intersectionArea = intersection.Width * intersection.Height;
		var areaA = a.Width * a.Height;
		var areaB = b.Width * b.Height;
		var unionArea = areaA + areaB - intersectionArea;
		return (double)intersectionArea / unionArea;
	}

	private double GetRandomDouble(double minimum, double maximum)
	{
		var valuesRange = maximum - minimum;
		var random = settings.Random.NextDouble();
		return minimum + valuesRange * random;
	}
}