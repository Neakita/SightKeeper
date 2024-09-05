using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Model.DataSets.Weights;

/// <summary>
/// MemoryPackable version of <see cref="Weights"/>
/// </summary>
internal abstract class PackableWeights
{
	public DateTime CreationDate { get; }
	public ModelSize ModelSize { get; }
	public WeightsMetrics Metrics { get; }
	public Vector2<ushort> Resolution { get; }

	protected PackableWeights(
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution)
	{
		CreationDate = creationDate;
		ModelSize = modelSize;
		Metrics = metrics;
		Resolution = resolution;
	}
}