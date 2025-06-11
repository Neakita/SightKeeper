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

	public void RemoveExceedUnusedImages(DomainImageSet set)
	{
		var ranges = GetRangesToRemove(set);
		foreach (var range in ranges)
			_imageRepository.DeleteImagesRange(set, range.Start, range.Count);
	}

	private readonly ImageRepository _imageRepository;

	/// <returns>reversed ranges, i.e. first range has latest indexes</returns>
	private IEnumerable<Range> GetRangesToRemove(DomainImageSet set)
	{
		var unusedRanges = set.Images
			.Index()
			.Where(tuple => !tuple.Item.IsInUse)
			.Select(tuple => tuple.Index)
			.ToRanges()
			.Reverse();
		return ExcludeAllowed(unusedRanges);
	}

	private IEnumerable<Range> ExcludeAllowed(IEnumerable<Range> ranges)
	{
		int remainingExclusions = UnusedImagesLimit;
		using var enumerator = ranges.GetEnumerator();
		while (remainingExclusions > 0 && enumerator.MoveNext())
		{
			var range = enumerator.Current;
			remainingExclusions -= range.Count;
			if (remainingExclusions < 0)
			{
				// Too many were excluded. Keep everything that doesn't fit.
				yield return Range.FromCount(range.Start, -remainingExclusions);
				break;
			}
		}
		while (enumerator.MoveNext())
			yield return enumerator.Current;
	}
}