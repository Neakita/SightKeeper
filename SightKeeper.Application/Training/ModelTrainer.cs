using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Training;

public interface ModelTrainer<TModel> where TModel : Domain.Model.DataSet
{
	TModel? Model { get; set; }
	bool FromScratch { get; set; }
	int? MaxBatches { get; }
	IObservable<TrainingProgress> Progress { get; }

	Task<InternalTrainedWeights?> TrainAsync(ModelConfig config, CancellationToken cancellationToken = default);
}