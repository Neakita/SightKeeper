using System.Buffers;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Data.ImageSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class ClassifierDataSetFormatter : MemoryPackFormatter<ClassifierDataSet>
{
	public required ImageLookupper ImageLookupper { get; init; }
	public required ClassifierDataSetWrapper SetWrapper { get; init; }

	public override void Serialize<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		scoped ref ClassifierDataSet? dataSet)
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

	public override void Deserialize(ref MemoryPackReader reader, scoped ref ClassifierDataSet? set)
	{
		if (reader.PeekIsNull())
		{
			set = null;
			return;
		}
		var inMemorySet = CreateInMemorySet();
		set = SetWrapper.Wrap(inMemorySet);
		DataSetGeneralDataFormatter.ReadGeneralData(ref reader, inMemorySet);
		TagsFormatter.ReadTags(ref reader, inMemorySet.TagsLibrary);
		ReadAssets(ref reader, inMemorySet);
		WeightsFormatter.ReadWeights(ref reader, inMemorySet.WeightsLibrary, inMemorySet.TagsLibrary.Tags);
	}

	private static void WriteAssets<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		IReadOnlyCollection<ClassifierAsset> assets,
		Dictionary<Tag, byte> tagIndexes)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteCollectionHeader(assets.Count);
		foreach (var asset in assets)
		{
			var imageId = asset.Image.GetId();
			var tagIndex = tagIndexes[asset.Tag];
			writer.WriteUnmanaged(imageId, tagIndex, asset.Usage);
		}
	}

	private InMemoryClassifierDataSet CreateInMemorySet()
	{
		StorableTagFactory tagFactory = new();
		StorableClassifierAssetFactory assetFactory = new();
		StorableWeightsWrapper weightsWrapper = new();
		var inMemorySet = new InMemoryClassifierDataSet(tagFactory, assetFactory, weightsWrapper);
		tagFactory.TagsOwner = inMemorySet.TagsLibrary;
		assetFactory.TagsOwner = inMemorySet.TagsLibrary;
		return inMemorySet;
	}

	private void ReadAssets(ref MemoryPackReader reader, InMemoryClassifierDataSet set)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var assetsCount));
		set.AssetsLibrary.EnsureCapacity(assetsCount);
		for (int i = 0; i < assetsCount; i++)
		{
			reader.ReadUnmanaged(out Id imageId, out byte tagIndex, out AssetUsage usage);
			var image = ImageLookupper.GetImage(imageId);
			var asset = set.AssetsLibrary.MakeAsset(image);
			asset.Tag = set.TagsLibrary.Tags[tagIndex];
			asset.Usage = usage;
		}
	}
}