using System.Collections.ObjectModel;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;

namespace SightKeeper.Domain.Model.DataSets.Screenshots;

public sealed class ScreenshotsLibrary : ObservableObject
{
    public Id Id { get; private set; }

    public DataSet DataSet { get; private set; }

    public ushort? MaxQuantity
    {
        get => _maxQuantity;
        set => SetProperty(ref _maxQuantity, value);
    }

    public IReadOnlyCollection<Screenshot> Screenshots => _screenshots;

    public bool HasAnyScreenshots
    {
        get => _hasAnyScreenshots;
        private set => SetProperty(ref _hasAnyScreenshots, value);
    }

    internal ScreenshotsLibrary(DataSet dataSet)
    {
        DataSet = dataSet;
        _screenshots = new ObservableCollection<Screenshot>();
    }

    public Screenshot CreateScreenshot(byte[] content)
    {
        Screenshot screenshot = new(content, this);
        _screenshots.Add(screenshot);
        ClearExceed();
        HasAnyScreenshots = Screenshots.Any();
        return screenshot;
    }

    public void DeleteScreenshot(Screenshot screenshot)
    {
        if (!_screenshots.Remove(screenshot))
            ThrowHelper.ThrowInvalidOperationException("Screenshot not found");
        HasAnyScreenshots = Screenshots.Any();
        screenshot.Asset?.ClearItems();
    }

    public void DeleteScreenshot(int screenshotIndex)
    {
        var screenshot = _screenshots[screenshotIndex];
        _screenshots.RemoveAt(screenshotIndex);
        HasAnyScreenshots = Screenshots.Any();
        screenshot.Asset?.ClearItems();
    }
	
    private readonly ObservableCollection<Screenshot> _screenshots;
    private ushort? _maxQuantity;
    private bool _hasAnyScreenshots;

    private ScreenshotsLibrary()
    {
        DataSet = null!;
        _screenshots = null!;
    }

    private void ClearExceed()
    {
        if (MaxQuantity == null)
            return;
        var screenshotsToDelete = Screenshots
            .Select((item, index) => (item, index))
            .Where(screenshot => screenshot.item.Asset == null)
            .OrderByDescending(screenshot => screenshot.item.CreationDate)
            .Skip(MaxQuantity.Value)
            .OrderByDescending(screenshot => screenshot.index)
            .ToList();
        foreach (var screenshot in screenshotsToDelete)
            DeleteScreenshot(screenshot.index);
    }
}