using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training;

internal sealed class TypeSwitchTrainer(Trainer<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> detectorTrainer) : Trainer<ReadOnlyTag, ReadOnlyAsset>
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

	public Task TrainAsync(ReadOnlyDataSet<ReadOnlyTag, ReadOnlyAsset> data, CancellationToken cancellationToken)
	{
		return data switch
		{
			ReadOnlyDataSet<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> detectorData => detectorTrainer.TrainAsync(detectorData, cancellationToken),
			_ => throw new ArgumentOutOfRangeException(nameof(data))
		};
	}
}