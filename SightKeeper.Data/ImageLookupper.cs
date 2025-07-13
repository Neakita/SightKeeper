using FlakeId;
using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data;

internal interface ImageLookupper
{
	StorableImage GetImage(Id id);
}