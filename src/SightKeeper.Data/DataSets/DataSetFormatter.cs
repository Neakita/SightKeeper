using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets;

public sealed class DataSetFormatter : MemoryPackFormatter<DataSet<Tag, Asset>>
{
	public DataSetFormatter(
		DataSetFactory<Tag, ClassifierAsset> classifierFactory,
		DataSetFactory<Tag, ItemsAsset<DetectorItem>> detectorFactory,
		DataSetFactory<PoserTag, ItemsAsset<PoserItem>> poserFactory,
		AssetsFormatter<Asset> assetsFormatter)
	{
		_classifierFactory = classifierFactory;
		_detectorFactory = detectorFactory;
		_poserFactory = poserFactory;
		_assetsFormatter = assetsFormatter;
	}

	public override void Serialize<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		scoped ref DataSet<Tag, Asset>? set)
	{
		if (set == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		ushort unionTag = set switch
		{
			DataSet<Tag, ClassifierAsset> => 0,
			DataSet<PoserTag, ItemsAsset<PoserItem>> => 2,
			DataSet<Tag, ItemsAsset<DetectorItem>> => 1,
			_ => throw new ArgumentOutOfRangeException(nameof(set))
		};
		writer.WriteUnionHeader(unionTag);
		var tagIndexes = set.TagsLibrary.Tags.Index().ToDictionary(tuple => tuple.Item, tuple => (byte)tuple.Index);
		DataSetGeneralDataFormatter.WriteGeneralData(ref writer, set);
		TagsFormatter.WriteTags(ref writer, set.TagsLibrary.Tags);
		_assetsFormatter.Serialize(ref writer, set.AssetsLibrary.Assets, tagIndexes);
		WeightsFormatter.WriteWeights(ref writer, set.WeightsLibrary.Weights, tagIndexes);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref DataSet<Tag, Asset>? set)
	{
		if (reader.PeekIsNull())
		{
			set = null;
			return;
		}
		Guard.IsTrue(reader.TryReadUnionHeader(out var unionTag));
		set = unionTag switch
		{
			0 => _classifierFactory.CreateDataSet(),
			1 => _detectorFactory.CreateDataSet(),
			2 => _poserFactory.CreateDataSet(),
			_ => throw new ArgumentOutOfRangeException(nameof(unionTag), unionTag, null)
		};
		var innerSet = set.UnWrapDecorator<InMemoryDataSet<Tag, Asset>>();
		DataSetGeneralDataFormatter.ReadGeneralData(ref reader, innerSet);
		TagsFormatter.ReadTags(ref reader, innerSet.TagsLibrary);
		_assetsFormatter.Deserialize(ref reader, innerSet);
		WeightsFormatter.ReadWeights(ref reader, innerSet.WeightsLibrary, innerSet.TagsLibrary.Tags);
	}

	private readonly DataSetFactory<Tag, ClassifierAsset> _classifierFactory;
	private readonly DataSetFactory<Tag, ItemsAsset<DetectorItem>> _detectorFactory;
	private readonly DataSetFactory<PoserTag, ItemsAsset<PoserItem>> _poserFactory;
	private readonly AssetsFormatter<Asset> _assetsFormatter;
}