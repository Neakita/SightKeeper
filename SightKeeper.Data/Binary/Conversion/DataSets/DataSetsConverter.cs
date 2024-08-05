using System.Collections.Immutable;
using SightKeeper.Data.Binary.Conversion.DataSets.Classifier;
using SightKeeper.Data.Binary.Conversion.DataSets.Detector;
using SightKeeper.Data.Binary.Conversion.DataSets.Poser;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Classifier;
using SightKeeper.Data.Binary.DataSets.Detector;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class DataSetsConverter
{
	public DataSetsConverter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemWeightsDataAccess weightsDataAccess)
	{
		_classifierConverter = new ClassifierDataSetsConverter(screenshotsDataAccess, weightsDataAccess);
		_detectorConverter = new DetectorDataSetsConverter(screenshotsDataAccess, weightsDataAccess);
		_poserConverter = new PoserDataSetsConverter(screenshotsDataAccess, weightsDataAccess);
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
			PoserDataSet poserDataSet => _poserConverter.Convert(poserDataSet, session),
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
			SerializablePoserDataSet poserDataSet => _poserConverter.ConvertBack(poserDataSet, session),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};
	}

	private readonly DetectorDataSetsConverter _detectorConverter;
	private readonly ClassifierDataSetsConverter _classifierConverter;
	private readonly PoserDataSetsConverter _poserConverter;
}