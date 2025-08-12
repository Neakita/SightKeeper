using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.DataSets.Classifier;

public sealed class ClassifierDataSetFormatter(ImageLookupper imageLookupper, DataSetFactory<StorableClassifierDataSet> setFactory) : MemoryPackFormatter<StorableClassifierDataSet>
{
	public override void Serialize<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		scoped ref StorableClassifierDataSet? set)
	{
		if (set == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		var tagIndexes = set.TagsLibrary.Tags.Index().ToDictionary(tuple => tuple.Item, tuple => (byte)tuple.Index);
		DataSetGeneralDataFormatter.WriteGeneralData(ref writer, set);
		TagsFormatter.WriteTags(ref writer, set.TagsLibrary.Tags);
		WriteAssets(ref writer, set.AssetsLibrary.Assets, tagIndexes);
		WeightsFormatter.WriteWeights(ref writer, set.WeightsLibrary.Weights, tagIndexes);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref StorableClassifierDataSet? set)
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
		IReadOnlyCollection<StorableClassifierAsset> assets,
		Dictionary<StorableTag, byte> tagIndexes)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(assets.Count);
		foreach (var asset in assets)
		{
			var imageId = asset.Image.Id;
			var tagIndex = tagIndexes[asset.Tag];
			writer.WriteUnmanaged(imageId, tagIndex, asset.Usage);
		}
	}

	private void ReadAssets(ref MemoryPackReader reader, StorableClassifierDataSet set)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var assetsCount));
		set.AssetsLibrary.EnsureCapacity(assetsCount);
		for (int i = 0; i < assetsCount; i++)
		{
			reader.ReadUnmanaged(out Id imageId, out byte tagIndex, out AssetUsage usage);
			var image = imageLookupper.GetImage(imageId);
			var asset = set.AssetsLibrary.MakeAsset(image);
			var tag = set.TagsLibrary.Tags[tagIndex];
			asset.Innermost.Tag = tag;
			tag.AddUser(asset);
			asset.Innermost.Usage = usage;
		}
	}
}