using System.Collections.Immutable;
using SightKeeper.Data.Binary.Conversion.DataSets.Classifier;
using SightKeeper.Data.Binary.Conversion.DataSets.Detector;
using SightKeeper.Data.Binary.Conversion.DataSets.Poser2D;
using SightKeeper.Data.Binary.Conversion.DataSets.Poser3D;
using SightKeeper.Data.Binary.DataSets.Poser3D;
using SightKeeper.Data.Binary.Services;
using ClassifierDataSet = SightKeeper.Data.Binary.DataSets.Classifier.ClassifierDataSet;
using DataSet = SightKeeper.Data.Binary.DataSets.DataSet;
using DetectorDataSet = SightKeeper.Data.Binary.DataSets.Detector.DetectorDataSet;
using Poser2DDataSet = SightKeeper.Data.Binary.DataSets.Poser2D.Poser2DDataSet;

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

	public ImmutableArray<DataSet> Convert(IEnumerable<Domain.Model.DataSets.DataSet> dataSets, ConversionSession session)
	{
		return dataSets.Select(dataSet => Convert(dataSet, session)).ToImmutableArray();
	}

	public DataSet Convert(Domain.Model.DataSets.DataSet dataSet, ConversionSession session)
	{
		return dataSet switch
		{
			Domain.Model.DataSets.Classifier.ClassifierDataSet classifierDataSet => _classifierConverter.Convert(classifierDataSet, session),
			Domain.Model.DataSets.Detector.DetectorDataSet detectorDataSet => _detectorConverter.Convert(detectorDataSet, session),
			Domain.Model.DataSets.Poser2D.Poser2DDataSet poserDataSet => _poser2DConverter.Convert(poserDataSet, session),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};
	}

	public HashSet<Domain.Model.DataSets.DataSet> ConvertBack(
		ImmutableArray<DataSet> dataSets,
		ReverseConversionSession session)
	{
		return dataSets.Select(dataSet => ConvertBack(dataSet, session)).ToHashSet();
	}

	public Domain.Model.DataSets.DataSet ConvertBack(DataSet dataSet, ReverseConversionSession session)
	{
		return dataSet switch
		{
			ClassifierDataSet classifierDataSet => _classifierConverter.ConvertBack(classifierDataSet, session),
			DetectorDataSet detectorDataSet => _detectorConverter.ConvertBack(detectorDataSet, session),
			Poser2DDataSet poserDataSet => _poser2DConverter.ConvertBack(poserDataSet, session),
			Poser3DDataSet poser3DDataSet => _poser3DConverter.ConvertBack(poser3DDataSet, session),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};
	}

	private readonly DetectorDataSetsConverter _detectorConverter;
	private readonly ClassifierDataSetsConverter _classifierConverter;
	private readonly Poser2DDataSetsConverter _poser2DConverter;
	private readonly Poser3DDataSetsConverter _poser3DConverter;
}