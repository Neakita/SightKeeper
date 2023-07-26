using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Annotating;

public interface Screenshoter
{
	IObservable<Screenshot> Screenshoted { get; }
	IObservable<Screenshot> ScreenshotRemoved { get; }
	Domain.Model.Model? Model { get; set; }
	bool IsEnabled { get; set; }
	byte ScreenshotsPerSecond { get; set; }
	ushort? MaxImages { get; set; }
}