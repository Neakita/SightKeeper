using SightKeeper.Application.Training.Data;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Application.Training;

public sealed class TypeSwitchTrainer(Trainer<ItemsAssetData<AssetItemData>> detectorTrainer) : Trainer<AssetData>
{
	public Vector2<ushort> ImageSize
	{
		get;
		set
		{
			field = value;
			detectorTrainer.ImageSize = value;
		}
	}

	public Task<Weights> TrainAsync(TrainData<AssetData> data, CancellationToken cancellationToken)
	{
		return data switch
		{
			TrainDataValue<ItemsAssetData<AssetItemData>> detectorData => detectorTrainer.TrainAsync(detectorData, cancellationToken),
			_ => throw new ArgumentOutOfRangeException(nameof(data))
		};
	}
}