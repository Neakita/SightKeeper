using SightKeeper.Application.Training.Data;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Application.Training;

public interface Trainer<in TAsset>
{
	Vector2<ushort> ImageSize { get; set; }
	Task<Weights> TrainAsync(TrainData<TAsset> data, CancellationToken cancellationToken);
}