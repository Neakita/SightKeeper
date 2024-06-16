namespace SightKeeper.Domain.Model.DataSets;

public abstract class Weights
{
	public DateTime CreationDate { get; }
	public ModelSize Size { get; }
	public WeightsMetrics Metrics { get; }

	protected Weights(ModelSize size, WeightsMetrics metrics)
	{
		CreationDate = DateTime.Now;
		Size = size;
		Metrics = metrics;
	}
}