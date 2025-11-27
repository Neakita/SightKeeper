using SightKeeper.Domain;

namespace SightKeeper.Application.Training.RFDETR;

internal sealed class RFDETRTrainingOptions : TrainingOptions
{
	public byte BatchSize { get; set; } = 4;
	public byte GradientAccumulationSteps { get; set; } = 4;
	public ushort Resolution { get; set; } = 320;
	public RFDETRModel Model { get; set; } = RFDETRModel.Nano;
	public ushort Epochs { get; set; } = 100;

	string TrainingOptions.Model => Model.Name;

	Vector2<ushort> TrainingOptions.Resolution => new(Resolution, Resolution);
}