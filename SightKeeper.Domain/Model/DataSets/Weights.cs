namespace SightKeeper.Domain.Model.DataSets;

public abstract class Weights
{
	public DateTime CreationDate { get; }
	public ModelSize Size { get; }
	public WeightsMetrics Metrics { get; }
	public abstract WeightsLibrary Library { get; }
	public abstract DataSet DataSet { get; }

	public abstract bool Contains(Tag tag);

	protected Weights(ModelSize size, WeightsMetrics metrics)
	{
		CreationDate = DateTime.Now;
		Size = size;
		Metrics = metrics;
	}
}