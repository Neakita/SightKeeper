using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Training;

public interface ModelTrainer<TDataSet> where TDataSet : DataSet
{
	IObservable<TrainingProgress> Progress { get; }

	Task<Weights?> TrainFromScratchAsync(
		TDataSet dataSet,
		ModelSize size,
		ushort epochs,
		CancellationToken cancellationToken = default);

	Task<Weights?> ResumeTrainingAsync(
		TDataSet dataSet,
		CancellationToken cancellationToken = default);
}