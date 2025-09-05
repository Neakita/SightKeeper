using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Application.Training;

public interface Trainer<in TTag, in TAsset>
{
	Vector2<ushort> ImageSize { get; set; }
	Task<WeightsData> TrainAsync(ReadOnlyDataSet<TTag, TAsset> data, CancellationToken cancellationToken);
}