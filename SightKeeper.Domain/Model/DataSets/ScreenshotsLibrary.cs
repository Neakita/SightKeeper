using System.Collections;
using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

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
	internal abstract ImmutableArray<Screenshot> ClearExceed();
	protected internal abstract Screenshot CreateScreenshot();
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}

public abstract class ScreenshotsLibrary<TScreenshot> : ScreenshotsLibrary, IReadOnlyCollection<TScreenshot> where TScreenshot : Screenshot
{
    public override int Count => _screenshots.Count;

    public override IEnumerator<TScreenshot> GetEnumerator() => _screenshots.GetEnumerator();

    internal void DeleteScreenshot(TScreenshot screenshot)
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

    protected internal abstract override TScreenshot CreateScreenshot();

    protected void AddScreenshot(TScreenshot screenshot)
    {
	    Guard.IsTrue(_screenshots.Add(screenshot));
    }

    private readonly SortedSet<TScreenshot> _screenshots = new(ScreenshotsDateComparer.Instance);
}

public sealed class AssetScreenshotsLibrary<TAsset> : ScreenshotsLibrary<Screenshot<TAsset>> where TAsset : Asset
{
	public override DataSet DataSet { get; }

	public AssetScreenshotsLibrary(DataSet dataSet)
	{
		DataSet = dataSet;
	}

	protected internal override Screenshot<TAsset> CreateScreenshot()
	{
		Screenshot<TAsset> screenshot = new(this);
		AddScreenshot(screenshot);
		return screenshot;
	}
}