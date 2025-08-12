using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Data.DataSets.Detector;

public sealed class DetectorDataSetFormatter(ImageLookupper imageLookupper, DataSetFactory<StorableDetectorDataSet> setFactory) : MemoryPackFormatter<StorableDetectorDataSet>
{
	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref StorableDetectorDataSet? dataSet)
	{
		if (dataSet == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		var tagIndexes = dataSet.TagsLibrary.Tags.Index().ToDictionary(tuple => tuple.Item, tuple => (byte)tuple.Index);
		DataSetGeneralDataFormatter.WriteGeneralData(ref writer, dataSet);
		TagsFormatter.WriteTags(ref writer, dataSet.TagsLibrary.Tags);
		WriteAssets(ref writer, dataSet.AssetsLibrary.Assets, tagIndexes);
		WeightsFormatter.WriteWeights(ref writer, dataSet.WeightsLibrary.Weights, tagIndexes);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref StorableDetectorDataSet? set)
	{
		if (reader.PeekIsNull())
		{
			set = null;
			return;
		}
		set = setFactory.CreateDataSet();
		var innerSet = set.Innermost;
		DataSetGeneralDataFormatter.ReadGeneralData(ref reader, innerSet);
		TagsFormatter.ReadTags(ref reader, innerSet.TagsLibrary);
		ReadAssets(ref reader, innerSet);
		WeightsFormatter.ReadWeights(ref reader, innerSet.WeightsLibrary, innerSet.TagsLibrary.Tags);
	}

	private static void WriteAssets<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<StorableItemsAsset<StorableDetectorItem>> assets,
		Dictionary<StorableTag, byte> tagIndexes)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(assets.Count);
		foreach (var asset in assets)
		{
			var imageId = asset.Image.Id;
			writer.WriteUnmanaged(imageId, asset.Usage);
			WriteItems(ref writer, asset.Items, tagIndexes);
		}
	}

	private static void WriteItems<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<StorableDetectorItem> items,
		Dictionary<StorableTag, byte> tagIndexes)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(items.Count);
		foreach (var item in items)
		{
			var tagIndex = tagIndexes[item.Tag];
			writer.WriteUnmanaged(tagIndex, item.Bounding);
		}
	}

	private void ReadAssets(ref MemoryPackReader reader, StorableDetectorDataSet set)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var assetsCount));
		set.AssetsLibrary.EnsureCapacity(assetsCount);
		for (int i = 0; i < assetsCount; i++)
		{
			reader.ReadUnmanaged(out Id imageId, out AssetUsage usage);
			var image = imageLookupper.GetImage(imageId);
			var asset = set.AssetsLibrary.MakeAsset(image);
			asset.Innermost.Usage = usage;
			ReadItems(ref reader, set.TagsLibrary.Tags, asset);
		}
	}

	private static void ReadItems(ref MemoryPackReader reader, IReadOnlyList<StorableTag> tags, ItemsOwner<StorableDetectorItem> itemsOwner)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var itemsCount));
		for (int i = 0; i < itemsCount; i++)
		{
			reader.ReadUnmanaged(out byte tagIndex, out Bounding bounding);
			var tag = tags[tagIndex];
			var item = itemsOwner.MakeItem(tag);
			tag.AddUser(item);
			item.Innermost.Bounding = bounding;
		}
	}
}