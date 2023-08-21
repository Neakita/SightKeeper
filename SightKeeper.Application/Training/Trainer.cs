using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Training;

public interface Trainer<TAsset> where TAsset : Asset
{
	IObservable<TrainingProgress> Progress { get; }

	Task<Weights?> TrainFromScratchAsync(
		DataSet<TAsset> dataSet,
		ModelSize size,
		ushort epochs,
		CancellationToken cancellationToken = default);

	Task<Weights?> ResumeTrainingAsync(
		DataSet<TAsset> dataSet,
		CancellationToken cancellationToken = default);
}