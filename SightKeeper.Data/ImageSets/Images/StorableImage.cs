using FlakeId;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

public interface StorableImage : ManagedImage, IDisposable
{
	Id Id { get; }

    void AddAsset(Asset asset);
    void RemoveAsset(Asset asset);
    void DeleteData();
}