namespace SightKeeper.Application.Annotating;

public interface StreamDataSetScreenshoter
{
    Domain.Model.DataSet.DataSet? DataSet { get; set; }
    bool IsEnabled { get; set; }
    byte ScreenshotsPerSecond { get; set; }
}