using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Annotation;

public abstract class ClassifierAnnotator
{
	protected ClassifierAnnotator(AssetsMaker assetsMaker)
	{
		_assetsMaker = assetsMaker;
	}

	public virtual void SetTag(AssetsLibrary<ClassifierAsset> assetsLibrary, Image image, Tag tag)
	{
		var asset = _assetsMaker.GetOrMakeAsset(assetsLibrary, image);
		asset.Tag = tag;
	}

	public virtual void DeleteAsset(AssetsLibrary<ClassifierAsset> assetsLibrary, Image image)
	{
		assetsLibrary.DeleteAsset(image);
	}

	private readonly AssetsMaker _assetsMaker;
}