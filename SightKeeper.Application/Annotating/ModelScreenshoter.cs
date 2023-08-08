using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Application.Annotating;

public sealed class ModelScreenshoter
{
    public Domain.Model.Model? Model
    {
        get => _model;
        set
        {
            _model = value;
            _screenshoter.Library = value?.ScreenshotsLibrary;
            _screenCapture.Resolution = _model?.Resolution;
            _screenCapture.Game = _model?.Game;
        }
    }

    public ModelScreenshoter(ScreenCapture screenCapture, Screenshoter screenshoter, GamesService gamesService)
    {
        _screenCapture = screenCapture;
        _screenshoter = screenshoter;
        _gamesService = gamesService;
    }

    public void MakeScreenshot()
    {
        if (!GetCanMakeScreenshot(out var message))
            ThrowHelper.ThrowInvalidOperationException("Can't make screenshot: " + message);
        _screenshoter.MakeScreenshot();
    }

    [MemberNotNullWhen(true, nameof(Model))]
    public bool GetCanMakeScreenshot([NotNullWhen(false)] out string? message)
    {
        if (Model == null)
            message = "Model is not set";
        else if (!_screenshoter.GetCanMakeScreenshot(out message))
            return false;
        else if (Model.Game != null && !_gamesService.IsGameActive(Model.Game))
            message = $"Game \"{Model.Game}\" is inactive";
        else message = null;
        return message == null;
    }

    private readonly ScreenCapture _screenCapture;
    private readonly Screenshoter _screenshoter;
    private readonly GamesService _gamesService;
    private Domain.Model.Model? _model;
}