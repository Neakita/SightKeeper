using System.Collections.Immutable;
using SightKeeper.Data.Binary.Conversion.DataSets.Classifier;
using SightKeeper.Data.Binary.Conversion.DataSets.Detector;
using SightKeeper.Data.Binary.Conversion.DataSets.Poser2D;
using SightKeeper.Data.Binary.Conversion.DataSets.Poser3D;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Classifier;
using SightKeeper.Data.Binary.DataSets.Detector;
using SightKeeper.Data.Binary.DataSets.Poser2D;
using SightKeeper.Data.Binary.DataSets.Poser3D;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class DataSetsConverter
{
	public DataSetsConverter(
		FileSystemScreenshotsDataAccess screenshotsDataAccess,
		FileSystemWeightsDataAccess weightsDataAccess)
	{
		WeightsConverter weightsConverter = new(weightsDataAccess);
		_classifierConverter = new ClassifierDataSetsConverter(screenshotsDataAccess, weightsDataAccess, weightsConverter);
		_detectorConverter = new DetectorDataSetsConverter(screenshotsDataAccess, weightsDataAccess, weightsConverter);
		_poser2DConverter = new Poser2DDataSetsConverter(screenshotsDataAccess, weightsDataAccess, weightsConverter);
		_poser3DConverter = new Poser3DDataSetsConverter(screenshotsDataAccess, weightsDataAccess, weightsConverter);
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
			Poser2DDataSet poserDataSet => _poser2DConverter.Convert(poserDataSet, session),
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
			SerializablePoser2DDataSet poserDataSet => _poser2DConverter.ConvertBack(poserDataSet, session),
			SerializablePoser3DDataSet poser3DDataSet => _poser3DConverter.ConvertBack(poser3DDataSet, session),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};
	}

	private readonly DetectorDataSetsConverter _detectorConverter;
	private readonly ClassifierDataSetsConverter _classifierConverter;
	private readonly Poser2DDataSetsConverter _poser2DConverter;
	private readonly Poser3DDataSetsConverter _poser3DConverter;
}