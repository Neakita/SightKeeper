using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Training;

public sealed class TrainingProgress
{
    public WeightsMetrics WeightsMetrics { get; }

    public TrainingProgress(WeightsMetrics weightsMetrics)
    {
	    WeightsMetrics = weightsMetrics;
    }

    public override string ToString() => WeightsMetrics.ToString();
}