using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Annotating;

public sealed class DataSetScreenshoter
{
    public DataSet? DataSet
    {
        get => _dataSet;
        set
        {
            _dataSet = value;
            _screenshoter.Library = value?.Screenshots;
            _screenCapture.Resolution = _dataSet?.Resolution;
            _screenCapture.Game = _dataSet?.Game;
        }
    }

    public DataSetScreenshoter(ScreenCapture screenCapture, Screenshoter screenshoter, GamesService gamesService)
    {
        _screenCapture = screenCapture;
        _screenshoter = screenshoter;
        _gamesService = gamesService;
    }

    public void MakeScreenshot()
    {
        if (!CanMakeScreenshot(out var message))
            ThrowHelper.ThrowInvalidOperationException("Can't make screenshot: " + message);
        _screenshoter.MakeScreenshot();
    }

    // ISSUE SRP violation
    [MemberNotNullWhen(true, nameof(DataSet))]
    public bool CanMakeScreenshot([NotNullWhen(false)] out string? message)
    {
        if (DataSet == null)
            message = $"{nameof(DataSet)} is not set";
        else if (!_screenshoter.GetCanMakeScreenshot(out message))
            return false;
        else if (DataSet.Game != null && !_gamesService.IsGameActive(DataSet.Game))
            message = $"Game \"{DataSet.Game}\" is inactive";
        else message = null;
        return message == null;
    }

    private readonly ScreenCapture _screenCapture;
    private readonly Screenshoter _screenshoter;
    private readonly GamesService _gamesService;
    private DataSet? _dataSet;
}