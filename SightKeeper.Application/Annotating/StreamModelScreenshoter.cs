namespace SightKeeper.Application.Annotating;

public interface StreamModelScreenshoter
{
    Domain.Model.Model? Model { get; set; }
    bool IsEnabled { get; set; }
    byte ScreenshotsPerSecond { get; set; }
}