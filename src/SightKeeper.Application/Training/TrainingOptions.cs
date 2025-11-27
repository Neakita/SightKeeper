using SightKeeper.Domain;

namespace SightKeeper.Application.Training;

public interface TrainingOptions
{
	ushort Epochs { get; }
	string Model { get; }
	Vector2<ushort> Resolution { get; }
}