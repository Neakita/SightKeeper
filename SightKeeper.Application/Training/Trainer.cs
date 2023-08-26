using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Training;

public interface Trainer
{
	IObservable<TrainingProgress> Progress { get; }

	Task<Weights?> TrainFromScratchAsync(
		Domain.Model.DataSet dataSet,
		ModelSize size,
		ushort epochs,
		CancellationToken cancellationToken = default);

	Task<Weights?> ResumeTrainingAsync(
		Domain.Model.DataSet dataSet,
		CancellationToken cancellationToken = default);
}