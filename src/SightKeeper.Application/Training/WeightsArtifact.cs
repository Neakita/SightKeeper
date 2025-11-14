using SightKeeper.Application.Training.RFDETR;

namespace SightKeeper.Application.Training;

public sealed class WeightsArtifact
{
	public string FileName { get; init; } = string.Empty;
	public EpochResult? EpochResult { get; set; }
}