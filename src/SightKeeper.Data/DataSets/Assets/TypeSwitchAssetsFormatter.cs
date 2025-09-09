using System.Buffers;
using MemoryPack;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Assets;

public sealed class TypeSwitchAssetsFormatter : AssetsFormatter<Asset>
{
	public TypeSwitchAssetsFormatter(
		AssetsFormatter<ClassifierAsset> classifierAssetsFormatter,
		AssetsFormatter<ItemsAsset<DetectorItem>> itemsAssetFormatter)
	{
		_classifierAssetsFormatter = classifierAssetsFormatter;
		_itemsAssetFormatter = itemsAssetFormatter;
	}

	public void Serialize<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<Asset> assets,
		Dictionary<Tag, byte> tagIndexes)
		where TBufferWriter : IBufferWriter<byte>
	{
		switch (assets)
		{
			case IReadOnlyCollection<ClassifierAsset> classifierAssets:
				_classifierAssetsFormatter.Serialize(ref writer, classifierAssets, tagIndexes);
				break;
			case IReadOnlyCollection<ItemsAsset<DetectorItem>> itemsAssets:
				_itemsAssetFormatter.Serialize(ref writer, itemsAssets, tagIndexes);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(assets));
		}
	}

	public void Deserialize(ref MemoryPackReader reader, DataSet<Tag, Asset> set)
	{
		switch (set)
		{
			case DataSet<Tag, ClassifierAsset> classifierSet:
				_classifierAssetsFormatter.Deserialize(ref reader, classifierSet);
				break;
			case DataSet<Tag, ItemsAsset<DetectorItem>> itemsSet:
				_itemsAssetFormatter.Deserialize(ref reader, itemsSet);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(set));
		}
	}

	private readonly AssetsFormatter<ClassifierAsset> _classifierAssetsFormatter;
	private readonly AssetsFormatter<ItemsAsset<DetectorItem>> _itemsAssetFormatter;
}