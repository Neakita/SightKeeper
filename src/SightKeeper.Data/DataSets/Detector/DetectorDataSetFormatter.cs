using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector;

public sealed class DetectorDataSetFormatter(ImageLookupper imageLookupper, DataSetFactory<DetectorDataSet> setFactory) : MemoryPackFormatter<DetectorDataSet>
{
	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref DetectorDataSet? dataSet)
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

	public override void Deserialize(ref MemoryPackReader reader, scoped ref DetectorDataSet? set)
	{
		if (reader.PeekIsNull())
		{
			set = null;
			return;
		}
		set = setFactory.CreateDataSet();
		var innerSet = set.UnWrapDecorator<InMemoryDetectorDataSet>();
		DataSetGeneralDataFormatter.ReadGeneralData(ref reader, innerSet);
		TagsFormatter.ReadTags(ref reader, innerSet.TagsLibrary);
		ReadAssets(ref reader, innerSet);
		WeightsFormatter.ReadWeights(ref reader, innerSet.WeightsLibrary, innerSet.TagsLibrary.Tags);
	}

	private static void WriteAssets<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<ItemsAsset<DetectorItem>> assets,
		Dictionary<Tag, byte> tagIndexes)
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
		IReadOnlyCollection<DetectorItem> items,
		Dictionary<Tag, byte> tagIndexes)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(items.Count);
		foreach (var item in items)
		{
			var tagIndex = tagIndexes[item.Tag];
			writer.WriteUnmanaged(tagIndex, item.Bounding);
		}
	}

	private void ReadAssets(ref MemoryPackReader reader, DetectorDataSet set)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var assetsCount));
		for (int i = 0; i < assetsCount; i++)
		{
			reader.ReadUnmanaged(out Id imageId, out AssetUsage usage);
			var image = imageLookupper.GetImage(imageId);
			var asset = set.AssetsLibrary.MakeAsset(image);
			var innermostAsset = asset.UnWrapDecorator<InMemoryItemsAsset<DetectorItem>>();
			innermostAsset.Usage = usage;
			ReadItems(ref reader, set.TagsLibrary.Tags, asset);
		}
	}

	private static void ReadItems(ref MemoryPackReader reader, IReadOnlyList<Tag> tags, ItemsOwner<DetectorItem> itemsOwner)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var itemsCount));
		for (int i = 0; i < itemsCount; i++)
		{
			reader.ReadUnmanaged(out byte tagIndex, out Bounding bounding);
			var tag = tags[tagIndex];
			var item = itemsOwner.MakeItem(tag);
			tag.AddUser(item);
			var innermostItem = item.UnWrapDecorator<InMemoryDetectorItem>();
			innermostItem.Bounding = bounding;
		}
	}
}