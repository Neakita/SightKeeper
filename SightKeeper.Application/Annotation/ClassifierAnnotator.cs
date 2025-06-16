using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Annotation;

public class ClassifierAnnotator
{
	public ClassifierAnnotator(AssetsMaker assetsMaker)
	{
		_assetsMaker = assetsMaker;
	}

	public virtual void SetTag(DomainAssetsLibrary<DomainClassifierAsset> assetsLibrary, DomainImage image, DomainTag tag)
	{
		var asset = _assetsMaker.GetOrMakeAsset(assetsLibrary, image);
		asset.Tag = tag;
	}

	public virtual void DeleteAsset(DomainAssetsLibrary<DomainClassifierAsset> assetsLibrary, DomainImage image)
	{
		assetsLibrary.DeleteAsset(image);
	}

	private readonly AssetsMaker _assetsMaker;
}