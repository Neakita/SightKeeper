using MemoryPack;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.DataSets.Detector;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Data.DataSets;

public sealed class DataSetFormatter : MemoryPackFormatter<DataSet>
{
	public required ClassifierDataSetFormatter ClassifierDataSetFormatter { get; init; }
	public required DetectorDataSetFormatter DetectorDataSetFormatter { get; init; }

	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref DataSet? value)
	{
		if (value == null)
		{
			writer.WriteNullUnionHeader();
			return;
		}

		switch (value)
		{
			case StorableClassifierDataSet classifierDataSet:
				writer.WriteUnionHeader(0);
				ClassifierDataSetFormatter.Serialize(ref writer, ref classifierDataSet!);
				break;
			case StorableDetectorDataSet detectorDataSet:
				writer.WriteUnionHeader(1);
				DetectorDataSetFormatter.Serialize(ref writer, ref detectorDataSet!);
				break;
			case Poser2DDataSet poser2DDataSet:
				writer.WriteUnionHeader(2);
				throw new NotImplementedException();
			case Poser3DDataSet poser3DDataSet:
				writer.WriteUnionHeader(3);
				throw new NotImplementedException();
			default:
				throw new ArgumentOutOfRangeException(nameof(value));
		}
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref DataSet? value)
	{
		if (!reader.TryReadUnionHeader(out var tag))
		{
			value = null;
			return;
		}
		value = tag switch
		{
			0 => reader.ReadValueWithFormatter<ClassifierDataSetFormatter, StorableClassifierDataSet>(ClassifierDataSetFormatter),
			1 => reader.ReadValueWithFormatter<DetectorDataSetFormatter, StorableDetectorDataSet>(DetectorDataSetFormatter),
			_ => throw new ArgumentOutOfRangeException()
		};
	}
}