using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public abstract class Weights
{
	public DateTime CreationDate { get; }
	public ModelSize ModelSize { get; }
	public WeightsMetrics Metrics { get; }
	public Vector2<ushort> Resolution { get; }
	public Composition? Composition { get; set; }
	public abstract WeightsLibrary Library { get; }
	public DataSet DataSet => Library.DataSet;

	public abstract bool Contains(Tag tag);

	protected Weights(DateTime creationDate, ModelSize size, WeightsMetrics metrics, Vector2<ushort> resolution, Composition? composition)
	{
		CreationDate = creationDate;
		ModelSize = size;
		Metrics = metrics;
		Resolution = resolution;
	}
}