using FlakeId;
using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data;

public interface ImageLookupper
{
	StorableImage GetImage(Id id);
}