using Vibrance;

namespace SightKeeper.Application.Training;

public interface TrainingArtifactsProvider
{
	ReadOnlyObservableList<TrainingArtifact> Artifacts { get; }
}