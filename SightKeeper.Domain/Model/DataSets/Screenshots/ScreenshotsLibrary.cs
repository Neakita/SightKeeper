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
	public abstract Screenshot AddScreenshot(DateTime creationDate);
	internal abstract ImmutableArray<Screenshot> ClearExceed();
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

    public override Screenshot<TAsset> AddScreenshot(DateTime creationDate)
    {
	    Screenshot<TAsset> screenshot = new(this, creationDate);
	    Guard.IsTrue(_screenshots.Add(screenshot));
	    return screenshot;
    }

    internal void DeleteScreenshot(Screenshot<TAsset> screenshot)
    {
        Guard.IsTrue(_screenshots.Remove(screenshot));
    }

    internal override ImmutableArray<Screenshot> ClearExceed()
    {
	    if (MaxQuantity == null)
		    return ImmutableArray<Screenshot>.Empty;
	    var screenshotWithoutAssetCount = _screenshots.Count - DataSet.Assets.Count;
	    var exceedAmount = screenshotWithoutAssetCount - MaxQuantity.Value;
	    if (exceedAmount <= 0)
		    return ImmutableArray<Screenshot>.Empty;
	    var builder = ImmutableArray.CreateBuilder<Screenshot>(exceedAmount);
	    var screenshotsToDelete = _screenshots.Where(screenshot => screenshot.Asset == null).Take(exceedAmount);
	    foreach (var screenshot in screenshotsToDelete)
	    {
		    Guard.IsTrue(_screenshots.Remove(screenshot));
		    builder.Add(screenshot);
	    }
	    return builder.ToImmutable();
    }

    private readonly SortedSet<Screenshot<TAsset>> _screenshots = new(ScreenshotsDateComparer.Instance);
}