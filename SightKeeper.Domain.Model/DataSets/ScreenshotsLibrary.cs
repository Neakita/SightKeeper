using System.Collections;
using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class ScreenshotsLibrary : IEnumerable<Screenshot>
{
    public ushort? MaxQuantity { get; set; }
    public DataSet DataSet { get; }

    public ScreenshotsLibrary(DataSet dataSet)
    {
	    DataSet = dataSet;
    }

    public void DeleteScreenshot(Screenshot screenshot)
    {
        if (!_screenshots.Remove(screenshot))
            ThrowHelper.ThrowInvalidOperationException("Screenshot not found");
        screenshot.Asset?.ClearItems();
    }

    public IEnumerator<Screenshot> GetEnumerator() => _screenshots.GetEnumerator();

    internal Screenshot CreateScreenshot()
    {
	    Screenshot screenshot = new(this);
	    _screenshots.Add(screenshot);
	    return screenshot;
    }

    internal ImmutableArray<Screenshot> ClearExceed()
    {
	    if (MaxQuantity == null)
		    return ImmutableArray<Screenshot>.Empty;
	    var exceedAmount = _screenshots.Count - MaxQuantity.Value;
	    if (exceedAmount <= 0)
		    return ImmutableArray<Screenshot>.Empty;
	    var screenshotsToDelete = _screenshots.Take(exceedAmount).ToImmutableArray();
	    foreach (var screenshot in screenshotsToDelete)
		    _screenshots.Remove(screenshot);
	    return screenshotsToDelete;
    }

    private readonly SortedSet<Screenshot> _screenshots = new(ScreenshotsDateComparer.Instance);

    IEnumerator IEnumerable.GetEnumerator()
    {
	    return GetEnumerator();
    }
}