namespace SightKeeper.Application.Annotating;

public interface StreamModelScreenshoter
{
    Domain.Model.DataSet? Model { get; set; }
    bool IsEnabled { get; set; }
    byte ScreenshotsPerSecond { get; set; }
}