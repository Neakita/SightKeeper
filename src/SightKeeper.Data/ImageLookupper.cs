using FlakeId;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

public interface ImageLookupper
{
	ManagedImage GetImage(Id id);
	bool ContainsImage(Id id);
}