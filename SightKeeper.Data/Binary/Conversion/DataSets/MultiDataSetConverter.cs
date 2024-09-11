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
	public MultiDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session)
	{
		_classifierConverter = new ClassifierDataSetConverter(screenshotsDataAccess, session);
		_detectorConverter = new DetectorDataSetConverter(screenshotsDataAccess, session);
		_poser2DConverter = new Poser2DDataSetConverter(screenshotsDataAccess, session);
		_poser3DConverter = new Poser3DDataSetConverter(screenshotsDataAccess, session);
	}

	public ImmutableArray<PackableDataSet> Convert(
		IEnumerable<DataSet> dataSets)
	{
		return dataSets.Select(Convert).ToImmutableArray();
	}

	public PackableDataSet Convert(
		DataSet dataSet)
	{
		return dataSet switch
		{
			ClassifierDataSet classifierDataSet => _classifierConverter.Convert(classifierDataSet),
			DetectorDataSet detectorDataSet => _detectorConverter.Convert(detectorDataSet),
			Poser2DDataSet poser2DDataSet => _poser2DConverter.Convert(poser2DDataSet),
			Poser3DDataSet poser3DDataSet => _poser3DConverter.Convert(poser3DDataSet),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};
	}

	private readonly ClassifierDataSetConverter _classifierConverter;
	private readonly DetectorDataSetConverter _detectorConverter;
	private readonly Poser2DDataSetConverter _poser2DConverter;
	private readonly Poser3DDataSetConverter _poser3DConverter;
}