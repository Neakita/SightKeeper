using FlakeId;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal interface StorableImage : Image
{
	Id Id { get; }

    void AddAsset(Asset asset);
    void RemoveAsset(Asset asset);
}