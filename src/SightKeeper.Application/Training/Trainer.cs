using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.Training;

public interface Trainer<in TTag, in TAsset>
{
	Vector2<ushort> ImageSize { get; set; }
	Task TrainAsync(ReadOnlyDataSet<TTag, TAsset> data, CancellationToken cancellationToken);
}