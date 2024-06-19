using System.Collections;
using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class ScreenshotsLibrary : IReadOnlyCollection<Screenshot>
{
	/// <summary>
	/// The maximum number of screenshots that can be contained in this library.
	/// If not specified (null), an unlimited number of images can be stored.
	/// </summary>
    public ushort? MaxQuantity { get; set; }
	
    /// <summary>
    /// The current number of screenshots stored.
    /// If the <see cref="MaxQuantity"/> is specified, it (generally) cannot exceed it.
    /// </summary>
    public int Count => _screenshots.Count;

    public void DeleteScreenshot(Screenshot screenshot)
    {
	    bool isRemoved = _screenshots.Remove(screenshot);
        Guard.IsTrue(isRemoved);
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
	    var builder = ImmutableArray.CreateBuilder<Screenshot>(exceedAmount);
	    for (int i = 0; i < exceedAmount; i++)
	    {
		    var oldestScreenshot = _screenshots.Min;
		    Guard.IsNotNull(oldestScreenshot);
		    Guard.IsTrue(_screenshots.Remove(oldestScreenshot));
		    builder.Add(oldestScreenshot);
	    }
	    return builder.ToImmutable();
    }

    private readonly SortedSet<Screenshot> _screenshots = new(ScreenshotsDateComparer.Instance);

    IEnumerator IEnumerable.GetEnumerator()
    {
	    return GetEnumerator();
    }
}