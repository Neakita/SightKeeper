using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Training;

public interface ModelTrainer<TModel> where TModel : Domain.Model.Model
{
	TModel? Model { get; set; }
	bool FromScratch { get; set; }
	int? MaxBatches { get; }
	IObservable<TrainingProgress> Progress { get; }

	Task<ModelWeights?> TrainAsync(CancellationToken cancellationToken = default);
}