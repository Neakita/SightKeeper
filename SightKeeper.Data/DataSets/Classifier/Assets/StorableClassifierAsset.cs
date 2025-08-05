using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Classifier.Assets;

public interface StorableClassifierAsset : ClassifierAsset
{
	new StorableTag Tag { get; set; }
	new StorableImage Image { get; }
	StorableClassifierAsset Innermost { get; }

	Tag ClassifierAsset.Tag
	{
		get => Tag;
		set => Tag = (StorableTag)value;
	}

	Image Asset.Image => Image;
}