using MemoryPack;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Data.DataSets;

internal sealed class DataSetFormatter : MemoryPackFormatter<DataSet>
{
	public DataSetFormatter(ImageLookupper imageLookupper)
	{
		_classifierDataSetFormatter = new ClassifierDataSetFormatter(imageLookupper);
	}

	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref DataSet? value)
	{
		if (value == null)
		{
			writer.WriteNullUnionHeader();
			return;
		}

		switch (value)
		{
			case ClassifierDataSet classifierDataSet:
				writer.WriteUnionHeader(0);
				_classifierDataSetFormatter.Serialize(ref writer, ref classifierDataSet!);
				break;
			case DetectorDataSet detectorDataSet:
				writer.WriteUnionHeader(1);
				throw new NotImplementedException();
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
			0 => reader.ReadValueWithFormatter<ClassifierDataSetFormatter, ClassifierDataSet>(_classifierDataSetFormatter),
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	private readonly ClassifierDataSetFormatter _classifierDataSetFormatter;
}