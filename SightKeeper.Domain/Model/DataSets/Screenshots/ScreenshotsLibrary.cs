using System.Collections;
using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Domain.Model.DataSets.Screenshots;

public abstract class ScreenshotsLibrary : IReadOnlyCollection<Screenshot>
{
	/// <summary>
	/// The maximum number of screenshots without asset that can be contained in this library.
	/// If not specified (null), an unlimited number can be stored.
	/// </summary>
	public ushort? MaxQuantity { get; set; }

	public abstract int Count { get; }
	
	public abstract DataSet DataSet { get; }

	public abstract IEnumerator<Screenshot> GetEnumerator();
	public abstract Screenshot CreateScreenshot(DateTime creationDate, out ImmutableArray<Screenshot> removedScreenshots);

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}

public sealed class ScreenshotsLibrary<TAsset> : ScreenshotsLibrary, IReadOnlyCollection<Screenshot<TAsset>> where TAsset : Asset
{
    public override int Count => _screenshots.Count;
    public override DataSet DataSet { get; }

    public ScreenshotsLibrary(DataSet dataSet)
    {
	    DataSet = dataSet;
    }

    public override IEnumerator<Screenshot<TAsset>> GetEnumerator() => _screenshots.GetEnumerator();

    public override Screenshot<TAsset> CreateScreenshot(DateTime creationDate, out ImmutableArray<Screenshot> removedScreenshots)
    {
	    Screenshot<TAsset> screenshot = new(this, creationDate);
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
	    var screenshotWithoutAssetCount = _screenshots.Count - DataSet.Assets.Count;
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