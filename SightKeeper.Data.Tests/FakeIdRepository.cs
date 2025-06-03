using FlakeId;
using SightKeeper.Data.Services;

namespace SightKeeper.Data.Tests;

internal sealed class FakeIdRepository<T> : IdRepository<T> where T : notnull
{
	public Dictionary<T, Id> Ids { get; } = new();

	public Id GetId(T item)
	{
		if (Ids.TryGetValue(item, out var id))
			return id;
		id = Id.Create();
		Ids.Add(item, id);
		return id;
	}

	public void AssociateId(T item, Id id)
	{
		Ids.Add(item, id);
	}
}