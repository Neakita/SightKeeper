using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class MultiDataSetConverter
{
	public MultiDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_classifierConverter = new ClassifierDataSetConverter(screenshotsDataAccess);
		_detectorConverter = new DetectorDataSetConverter(screenshotsDataAccess);
	}

	public PackableDataSet Convert(DataSet dataSet, ConversionSession session)
	{
		return dataSet switch
		{
			ClassifierDataSet classifierDataSet => _classifierConverter.Convert(classifierDataSet, session),
			DetectorDataSet detectorDataSet => _detectorConverter.Convert(detectorDataSet, session),
			Poser2DDataSet poser2DDataSet => throw new NotImplementedException(),
			Poser3DDataSet poser3DDataSet => throw new NotImplementedException(),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};
	}

	private readonly ClassifierDataSetConverter _classifierConverter;
	private readonly DetectorDataSetConverter _detectorConverter;
}