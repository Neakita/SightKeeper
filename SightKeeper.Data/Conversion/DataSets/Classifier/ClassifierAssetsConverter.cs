using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.Conversion.DataSets.Classifier;

internal sealed class ClassifierAssetsConverter
{
	public ClassifierAssetsConverter(FileSystemImageDataAccess imageDataAccess, ConversionSession session)
	{
		_imageDataAccess = imageDataAccess;
		_session = session;
	}

	public IEnumerable<PackableClassifierAsset> ConvertAssets(IEnumerable<ClassifierAsset> assets)
	{
		return assets.Select(ConvertAsset);
	}

	private readonly FileSystemImageDataAccess _imageDataAccess;
	private readonly ConversionSession _session;

	private PackableClassifierAsset ConvertAsset(ClassifierAsset asset)
	{
		var image = asset.Image;
		return new PackableClassifierAsset
		{
			TagIndex = _session.TagsIndexes[asset.Tag],
			Usage = asset.Usage,
			ImageId = _imageDataAccess.GetId(image)
		};
	}
}