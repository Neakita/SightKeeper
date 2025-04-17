using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Conversion.DataSets.Classifier;

internal sealed class ClassifierAssetsConverter
{
	public ClassifierAssetsConverter(FileSystemImageDataAccess imageDataAccess, ConversionSession session)
	{
		_imageDataAccess = imageDataAccess;
		_session = session;
	}

	public IEnumerable<PackableClassifierAsset> ConvertAssets(IEnumerable<KeyValuePair<Image, ClassifierAsset>> assetsPairs)
	{
		return assetsPairs.Select(ConvertAsset);
	}

	private readonly FileSystemImageDataAccess _imageDataAccess;
	private readonly ConversionSession _session;

	private PackableClassifierAsset ConvertAsset(KeyValuePair<Image, ClassifierAsset> assetPair)
	{
		var (image, asset) = assetPair;
		return new PackableClassifierAsset
		{
			TagIndex = _session.TagsIndexes[asset.Tag],
			ImageId = _imageDataAccess.GetId(image)
		};
	}
}