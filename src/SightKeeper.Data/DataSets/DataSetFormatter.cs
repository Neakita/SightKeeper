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
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Data.DataSets;

public sealed class DataSetFormatter : MemoryPackFormatter<DataSet>
{
	public DataSetFormatter(
		DataSetFactory<DataSet<ClassifierAsset>> classifierFactory,
		DataSetFactory<DataSet<ItemsAsset<DetectorItem>>> detectorFactory,
		DataSetFactory<DataSet<ItemsAsset<PoserItem>>> poserFactory,
		AssetsFormatter<Asset> assetsFormatter)
	{
		_classifierFactory = classifierFactory;
		_detectorFactory = detectorFactory;
		_poserFactory = poserFactory;
		_assetsFormatter = assetsFormatter;
	}

	public override void Serialize<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		scoped ref DataSet? set)
	{
		if (set == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		ushort unionTag = set switch
		{
			DataSet<ClassifierAsset> => 0,
			DataSet<ItemsAsset<DetectorItem>> => 1,
			DataSet<ItemsAsset<PoserItem>> => 2,
			_ => throw new ArgumentOutOfRangeException(nameof(set))
		};
		writer.WriteUnionHeader(unionTag);
		var tagIndexes = set.TagsLibrary.Tags.Index().ToDictionary(tuple => tuple.Item, tuple => (byte)tuple.Index);
		DataSetGeneralDataFormatter.WriteGeneralData(ref writer, set);
		TagsFormatter.WriteTags(ref writer, set.TagsLibrary.Tags);
		_assetsFormatter.Serialize(ref writer, set.AssetsLibrary.Assets, tagIndexes);
		WeightsFormatter.WriteWeights(ref writer, set.WeightsLibrary.Weights, tagIndexes);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref DataSet? set)
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
		var innerSet = set.UnWrapDecorator<InMemoryDataSet<Asset>>();
		DataSetGeneralDataFormatter.ReadGeneralData(ref reader, innerSet);
		TagsFormatter.ReadTags(ref reader, innerSet.TagsLibrary);
		_assetsFormatter.Deserialize(ref reader, innerSet);
		WeightsFormatter.ReadWeights(ref reader, innerSet.WeightsLibrary, innerSet.TagsLibrary.Tags);
	}

	private readonly DataSetFactory<DataSet<ClassifierAsset>> _classifierFactory;
	private readonly DataSetFactory<DataSet<ItemsAsset<DetectorItem>>> _detectorFactory;
	private readonly DataSetFactory<DataSet<ItemsAsset<PoserItem>>> _poserFactory;
	private readonly AssetsFormatter<Asset> _assetsFormatter;
}