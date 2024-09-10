using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class MultiDataSetConverter
{
	public MultiDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_classifierConverter = new ClassifierDataSetConverter(screenshotsDataAccess);
		_detectorConverter = new DetectorDataSetConverter(screenshotsDataAccess);
		_poser2DConverter = new Poser2DDataSetConverter(screenshotsDataAccess);
		_poser3DConverter = new Poser3DDataSetConverter(screenshotsDataAccess);
	}

	public ImmutableArray<PackableDataSet> Convert(
		IEnumerable<DataSet> dataSets,
		ConversionSession session)
	{
		return dataSets.Select(dataSet => Convert(dataSet, session)).ToImmutableArray();
	}

	public PackableDataSet Convert(
		DataSet dataSet,
		ConversionSession session)
	{
		return dataSet switch
		{
			ClassifierDataSet classifierDataSet => _classifierConverter.Convert(classifierDataSet, session),
			DetectorDataSet detectorDataSet => _detectorConverter.Convert(detectorDataSet, session),
			Poser2DDataSet poser2DDataSet => _poser2DConverter.Convert(poser2DDataSet, session),
			Poser3DDataSet poser3DDataSet => _poser3DConverter.Convert(poser3DDataSet, session),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};
	}

	private readonly ClassifierDataSetConverter _classifierConverter;
	private readonly DetectorDataSetConverter _detectorConverter;
	private readonly Poser2DDataSetConverter _poser2DConverter;
	private readonly Poser3DDataSetConverter _poser3DConverter;
}