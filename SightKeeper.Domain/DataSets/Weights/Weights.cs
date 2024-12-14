using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Domain.DataSets.Weights;

public abstract class Weights
{
	public DateTimeOffset CreationDate { get; }
	public ModelSize ModelSize { get; }
	public WeightsMetrics Metrics { get; }
	public Vector2<ushort> Resolution { get; }
	public ImageComposition? Composition { get; }

	protected Weights(DateTimeOffset creationDate, ModelSize size, WeightsMetrics metrics, Vector2<ushort> resolution, ImageComposition? composition)
	{
		CreationDate = creationDate;
		ModelSize = size;
		Metrics = metrics;
		Resolution = resolution;
		Composition = composition;
	}
}