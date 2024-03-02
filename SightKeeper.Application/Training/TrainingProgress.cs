using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Application.Training;

public sealed class TrainingProgress
{
    public WeightsMetrics Metrics { get; }

    public TrainingProgress(WeightsMetrics metrics)
    {
	    Metrics = metrics;
    }

    public override string ToString() => Metrics.ToString();
}