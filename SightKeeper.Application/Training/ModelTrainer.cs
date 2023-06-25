using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Training;

public interface ModelTrainer<TModel> where TModel : Model
{
	TModel? Model { get; set; }
	bool FromScratch { get; set; }
	int? MaxBatches { get; }
	IObservable<TrainingProgress> Progress { get; }

	Task<ModelWeights?> TrainAsync(CancellationToken cancellationToken = default);
}

public interface ModelTrainer : ModelTrainer<Model>
{
}