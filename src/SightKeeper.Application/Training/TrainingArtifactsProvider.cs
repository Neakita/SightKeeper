using Vibrance;

namespace SightKeeper.Application.Training;

public interface TrainingArtifactsProvider
{
	ReadOnlyObservableList<WeightsArtifact> Artifacts { get; }
}