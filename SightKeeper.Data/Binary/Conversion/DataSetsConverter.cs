using System.Collections.Immutable;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Classifier;
using SightKeeper.Data.Binary.DataSets.Detector;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class DataSetsConverter
{
	public DataSetsConverter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemDetectorWeightsDataAccess detectorWeightsDataAccess,
		FileSystemClassifierWeightsDataAccess classifierWeightsDataAccess)
	{
		_classifierConverter = new ClassifierDataSetsConverter(screenshotsDataAccess, classifierWeightsDataAccess);
		_detectorConverter = new DetectorDataSetsConverter(screenshotsDataAccess, detectorWeightsDataAccess);
	}

	public ImmutableArray<SerializableDataSet> Convert(IEnumerable<DataSet> dataSets, ConversionSession session)
	{
		return dataSets.Select(dataSet => Convert(dataSet, session)).ToImmutableArray();
	}

	public SerializableDataSet Convert(DataSet dataSet, ConversionSession session)
	{
		return dataSet switch
		{
			ClassifierDataSet classifierDataSet => _classifierConverter.Convert(classifierDataSet, session),
			DetectorDataSet detectorDataSet => _detectorConverter.Convert(detectorDataSet, session),
			PoserDataSet poserDataSet => throw new NotImplementedException(),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};
	}

	public HashSet<DataSet> ConvertBack(
		ImmutableArray<SerializableDataSet> dataSets,
		ReverseConversionSession session)
	{
		return dataSets.Select(dataSet => ConvertBack(dataSet, session)).ToHashSet();
	}

	public DataSet ConvertBack(SerializableDataSet dataSet, ReverseConversionSession session)
	{
		return dataSet switch
		{
			SerializableClassifierDataSet classifierDataSet => _classifierConverter.ConvertBack(classifierDataSet, session),
			SerializableDetectorDataSet detectorDataSet => _detectorConverter.ConvertBack(detectorDataSet, session),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};
	}

	private readonly DetectorDataSetsConverter _detectorConverter;
	private readonly ClassifierDataSetsConverter _classifierConverter;
}