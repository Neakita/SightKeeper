using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Domain.DataSets.Weights;

public abstract class Weights
{
	public DateTimeOffset CreationTimestamp { get; }
	public ModelSize ModelSize { get; }
	public WeightsMetrics Metrics { get; }
	public Vector2<ushort> Resolution { get; }
	public ImageComposition? Composition { get; }

	protected Weights(DateTimeOffset creationTimestamp, ModelSize size, WeightsMetrics metrics, Vector2<ushort> resolution, ImageComposition? composition)
	{
		CreationTimestamp = creationTimestamp;
		ModelSize = size;
		Metrics = metrics;
		Resolution = resolution;
		Composition = composition;
	}
}