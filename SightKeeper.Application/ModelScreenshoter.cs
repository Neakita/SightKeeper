using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Application;

public interface ModelScreenshoter
{
	Model? Model { get; set; }
	bool IsEnabled { get; set; }
	byte OnHoldFPS { get; set; }
	ushort MaxImages { get; set; }
}