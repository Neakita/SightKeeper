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
            if (_screenshoter.Library != null && value != null && _screenshoter.Library != value.ScreenshotsLibrary)
                ThrowHelper.ThrowInvalidOperationException($"{nameof(Screenshoter)} already has a different library");
            _screenshoter.Library = value?.ScreenshotsLibrary;
            _screenCapture.Resolution = _model?.Resolution;
            _screenCapture.Game = _model?.Game;
        }
    }

    public ModelScreenshoter(ScreenCapture screenCapture, Screenshoter screenshoter)
    {
        _screenCapture = screenCapture;
        _screenshoter = screenshoter;
    }

    public void MakeScreenshot()
    {
        Guard.IsNotNull(Model);
        _screenshoter.MakeScreenshot();
    }

    private readonly ScreenCapture _screenCapture;
    private readonly Screenshoter _screenshoter;
    private Domain.Model.Model? _model;
}