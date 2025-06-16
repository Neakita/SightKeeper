using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Annotation;

public class ClassifierAnnotator
{
	public ClassifierAnnotator(AssetsMaker assetsMaker)
	{
		_assetsMaker = assetsMaker;
	}

	public virtual void SetTag(DomainAssetsLibrary<DomainClassifierAsset> assetsLibrary, Image image, DomainTag tag)
	{
		var asset = _assetsMaker.GetOrMakeAsset(assetsLibrary, image);
		asset.Tag = tag;
	}

	public virtual void DeleteAsset(DomainAssetsLibrary<DomainClassifierAsset> assetsLibrary, Image image)
	{
		assetsLibrary.DeleteAsset(image);
	}

	private readonly AssetsMaker _assetsMaker;
}