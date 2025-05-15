using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing;

public sealed class ImagesCleaner
{
	public ushort UnusedImagesLimit { get; set; } = 500;

	public ImagesCleaner(ImageRepository imageRepository)
	{
		_imageRepository = imageRepository;
	}

	public void RemoveExceedUnusedImages(ImageSet set)
	{
		var ranges = GetRangesToRemove(set);
		for (var i = ranges.Count - 1; i >= 0; i--)
		{
			var range = ranges[i];
			_imageRepository.DeleteImagesRange(set, range.Start, range.Count);
		}
	}

	private readonly ImageRepository _imageRepository;

	private List<Range> GetRangesToRemove(ImageSet set)
	{
		var unusedRanges = set.Images
			.Index()
			.Where(tuple => !IsInUse(tuple.Item))
			.Select(tuple => tuple.Index)
			.ToRanges()
			.ToList();
		if (unusedRanges.Count == 0)
			return unusedRanges;
		RemoveLimitedLastIndexes(unusedRanges);
		return unusedRanges;
	}

	private bool IsInUse(Image image)
	{
		return image.Assets.Count > 0;
	}

	private void RemoveLimitedLastIndexes(List<Range> ranges)
	{
		int targetRemoves = UnusedImagesLimit;
		while (targetRemoves > 0 && ranges.Count > 0)
		{
			var index = ranges.Count - 1;
			var lastRange = ranges[index];
			ranges.RemoveAt(index);
			var lastRemovedCount = Math.Min(lastRange.Count, targetRemoves);
			targetRemoves -= lastRemovedCount;
			var modifiedRangeCount = lastRange.Count - lastRemovedCount;
			if (modifiedRangeCount > 0)
			{
				var modifiedRange = CreateRangeFromCount(lastRange.Start, modifiedRangeCount);
				ranges.Add(modifiedRange);
			}
		}
		
	}

	private static Range CreateRangeFromCount(int start, int count)
	{
		return new Range(start, start + count - 1);
	}
}