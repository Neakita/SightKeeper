using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class ItemsAssetsFormatter<TItem>(ItemsFormatter<TItem> itemsFormatter, ImageLookupper imageLookupper) : AssetsFormatter<ItemsAsset<TItem>>
{
	public void Serialize<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<ItemsAsset<TItem>> assets,
		Dictionary<Tag, byte> tagIndexes)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(assets.Count);
		foreach (var asset in assets)
		{
			var imageId = asset.Image.Id;
			writer.WriteUnmanaged(imageId, asset.Usage);
			itemsFormatter.WriteItems(ref writer, asset.Items, tagIndexes);
		}
	}

	public void Deserialize(ref MemoryPackReader reader, DataSet<Tag, ItemsAsset<TItem>> set)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var assetsCount));
		for (int i = 0; i < assetsCount; i++)
		{
			reader.ReadUnmanaged(out Id imageId, out AssetUsage usage);
			var image = imageLookupper.GetImage(imageId);
			var asset = set.AssetsLibrary.MakeAsset(image);
			var innermostAsset = asset.UnWrapDecorator<InMemoryItemsAsset<DetectorItem>>();
			innermostAsset.Usage = usage;
			itemsFormatter.ReadItems(ref reader, set.TagsLibrary.Tags, asset);
		}
	}
}