using FlakeId;

namespace SightKeeper.Data.Services;

public interface ReadIdRepository<in T>
{
	Id GetId(T item);
}