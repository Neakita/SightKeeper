using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Classifier;

internal interface StorableClassifierAsset : ClassifierAsset
{
	new StorableImage Image { get; }

	Image Asset.Image => Image;
}