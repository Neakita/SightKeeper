using FlakeId;

namespace SightKeeper.Data.Services;

public interface WriteIdRepository<in T>
{
	void AssociateId(T item, Id id);
}