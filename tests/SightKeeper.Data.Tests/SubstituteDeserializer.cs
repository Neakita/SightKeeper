using MemoryPack;
using SightKeeper.Data.Services;

namespace SightKeeper.Data.Tests;

internal sealed class SubstituteDeserializer<T>(Func<T> resultFactory) : Deserializer<T>
{
	public int CallsCounts { get; private set; }

	public T Deserialize(ref MemoryPackReader reader)
	{
		CallsCounts++;
		return resultFactory();
	}
}