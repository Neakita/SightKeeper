using SightKeeper.Data.Binary.Services;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal abstract class PoserDataSetConverter : DataSetConverter
{
	protected PoserDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}
}