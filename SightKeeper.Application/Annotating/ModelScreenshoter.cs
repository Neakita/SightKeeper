using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Application.Annotating;

public interface ModelScreenshoter
{
	Model? Model { get; set; }
	bool IsEnabled { get; set; }
	byte FramesPerSecond { get; set; }
	ushort MaxImages { get; set; }
}