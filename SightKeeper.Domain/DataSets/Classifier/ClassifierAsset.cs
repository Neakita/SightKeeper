using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Classifier;

public interface ClassifierAsset : Asset
{
	Tag Tag { get; set; }
}