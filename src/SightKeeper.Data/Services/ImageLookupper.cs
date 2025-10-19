using FlakeId;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

internal interface ImageLookupper
{
	ManagedImage GetImage(Id id);
	bool ContainsImage(Id id);
}