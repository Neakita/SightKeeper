using System.Collections;
using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class ScreenshotsLibrary<TScreenshot> : IReadOnlyCollection<TScreenshot> where TScreenshot : Screenshot
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

    public void DeleteScreenshot(TScreenshot screenshot)
    {
	    bool isRemoved = _screenshots.Remove(screenshot);
        Guard.IsTrue(isRemoved);
    }

    public IEnumerator<TScreenshot> GetEnumerator() => _screenshots.GetEnumerator();

    internal protected abstract TScreenshot CreateScreenshot();

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

    protected void AddScreenshot(TScreenshot screenshot)
    {
	    Guard.IsTrue(_screenshots.Add(screenshot));
    }

    private readonly SortedSet<TScreenshot> _screenshots = new(ScreenshotsDateComparer.Instance);

    IEnumerator IEnumerable.GetEnumerator()
    {
	    return GetEnumerator();
    }
}