using SightKeeper.Application.Training.RFDETR;

namespace SightKeeper.Application.Training;

public sealed class TrainingArtifact
{
	public string FileName { get; init; } = string.Empty;
	public EpochResult? EpochResult { get; set; }
}