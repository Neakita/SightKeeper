using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Application.Training;

public interface ModelTrainer<TModel> where TModel : Model
{
	bool IsRunning { get; }
	float? Progress { get; }
	uint? CurrentBatch { get; }
	uint? MaxBatches { get; }
	string? Status { get; }
	double? AverageLoss { get; }
	TimeSpan? TimeRemaining { get; }

	void BeginTraining(TModel model, bool fromScratch);
	void EndTraining();
}