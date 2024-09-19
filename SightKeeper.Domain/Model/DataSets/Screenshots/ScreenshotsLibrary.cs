using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Domain.Model.DataSets.Screenshots;

public abstract class ScreenshotsLibrary
{
	/// <summary>
	/// The maximum number of screenshots without asset that can be contained in this library.
	/// If not specified (null), an unlimited number can be stored.
	/// </summary>
	public ushort? MaxQuantity { get; set; }
	public abstract DataSet DataSet { get; }

	/// <remarks>
	/// Sorted by date: first is the earliest, last is the latest
	/// </remarks>
	public abstract IReadOnlyList<Screenshot> Screenshots { get; }

	public abstract Screenshot CreateScreenshot(DateTime creationDate, Vector2<ushort> resolution, out ImmutableArray<Screenshot> removedScreenshots);
}

public sealed class ScreenshotsLibrary<TAsset> : ScreenshotsLibrary where TAsset : Asset
{
    public override DataSet DataSet { get; }
    public override IReadOnlyList<Screenshot<TAsset>> Screenshots => _screenshots;

    public ScreenshotsLibrary(DataSet dataSet)
    {
	    DataSet = dataSet;
    }

    public override Screenshot<TAsset> CreateScreenshot(DateTime creationDate, Vector2<ushort> resolution, out ImmutableArray<Screenshot> removedScreenshots)
    {
	    Screenshot<TAsset> screenshot = new(this, creationDate, resolution);
	    if (_screenshots.Count != 0)
			Guard.IsGreaterThan(creationDate, _screenshots[^1].CreationDate);
	    _screenshots.Add(screenshot);
	    removedScreenshots = ClearExceed();
	    return screenshot;
    }

    internal void DeleteScreenshot(Screenshot<TAsset> screenshot)
    {
        Guard.IsTrue(_screenshots.Remove(screenshot));
    }

    private ImmutableArray<Screenshot> ClearExceed()
    {
	    if (MaxQuantity == null)
		    return ImmutableArray<Screenshot>.Empty;
	    var screenshotWithoutAssetCount = _screenshots.Count - DataSet.AssetsLibrary.Assets.Count;
	    var exceedAmount = screenshotWithoutAssetCount - MaxQuantity.Value;
	    if (exceedAmount <= 0)
		    return ImmutableArray<Screenshot>.Empty;
	    var builder = ImmutableArray.CreateBuilder<Screenshot>(exceedAmount);
	    var screenshotsToDelete =
		    _screenshots
			    .Select((screenshot, index) => (screenshot, index))
			    .Where(tuple => tuple.screenshot.Asset == null)
			    .Take(exceedAmount)
			    .ToRanges(tuple => tuple.index)
			    .OrderByDescending(tuple => tuple.start);
	    foreach (var (tuples, start, end) in screenshotsToDelete)
	    {
		    _screenshots.RemoveRange(start, end - start + 1);
		    builder.Capacity += tuples.Length;
		    foreach (var (screenshot, _) in tuples)
			    builder.Add(screenshot);
	    }
	    return builder.ToImmutable();
    }

    private readonly List<Screenshot<TAsset>> _screenshots = new();
}