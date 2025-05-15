using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.Conversion.DataSets.Classifier;

internal sealed class ClassifierAssetsConverter
{
	public ClassifierAssetsConverter(FileSystemImageRepository imageRepository, ConversionSession session)
	{
		_imageRepository = imageRepository;
		_session = session;
	}

	public IEnumerable<PackableClassifierAsset> ConvertAssets(IEnumerable<ClassifierAsset> assets)
	{
		return assets.Select(ConvertAsset);
	}

	private readonly FileSystemImageRepository _imageRepository;
	private readonly ConversionSession _session;

	private PackableClassifierAsset ConvertAsset(ClassifierAsset asset)
	{
		var image = asset.Image;
		return new PackableClassifierAsset
		{
			TagIndex = _session.TagsIndexes[asset.Tag],
			Usage = asset.Usage,
			ImageId = _imageRepository.GetId(image)
		};
	}
}