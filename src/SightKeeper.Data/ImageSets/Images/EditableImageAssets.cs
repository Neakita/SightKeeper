using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.ImageSets.Images;

internal interface EditableImageAssets
{
	void Add(Asset asset);
	void Remove(Asset asset);
}