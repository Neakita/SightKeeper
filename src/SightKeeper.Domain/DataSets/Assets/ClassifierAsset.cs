using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Assets;

public interface ClassifierAsset : Asset, TagUser
{
	Tag Tag { get; set; }
}