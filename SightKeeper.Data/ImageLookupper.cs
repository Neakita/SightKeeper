using FlakeId;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

internal interface ImageLookupper
{
	Image GetImage(Id id);
}