using SightKeeper.Data.Binary.Conversion.DataSets.Classifier;
using SightKeeper.Data.Binary.Conversion.DataSets.Detector;
using SightKeeper.Data.Binary.Conversion.DataSets.Poser2D;
using SightKeeper.Data.Binary.Conversion.DataSets.Poser3D;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class DataSetsConverter
{
	public DataSetsConverter(ConversionSession session, FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_classifierConverter = new ClassifierDataSetConverter(session, screenshotsDataAccess);
		_detectorConverter = new DetectorDataSetConverter(session, screenshotsDataAccess);
		_poser2DConverter = new Poser2DDataSetConverter(session, screenshotsDataAccess);
		_poser3DConverter = new Poser3DDataSetConverter(session, screenshotsDataAccess);
	}

	public IEnumerable<PackableDataSet> ConvertDataSets(IEnumerable<DataSet> dataSets)
	{
		return dataSets.Select(ConvertDataSet);
	}

	private readonly ClassifierDataSetConverter _classifierConverter;
	private readonly DetectorDataSetConverter _detectorConverter;
	private readonly Poser2DDataSetConverter _poser2DConverter;
	private readonly Poser3DDataSetConverter _poser3DConverter;

	private PackableDataSet ConvertDataSet(DataSet dataSet) => dataSet switch
	{
		ClassifierDataSet classifierDataSet => _classifierConverter.ConvertDataSet(classifierDataSet),
		DetectorDataSet detectorDataSet => _detectorConverter.ConvertDataSet(detectorDataSet),
		Poser2DDataSet poser2DDataSet => _poser2DConverter.ConvertDataSet(poser2DDataSet),
		Poser3DDataSet poser3DDataSet => _poser3DConverter.ConvertDataSet(poser3DDataSet),
		_ => throw new ArgumentOutOfRangeException(nameof(dataSet), dataSet, null)
	};
}