using MemoryPack;

namespace SightKeeper.Data.Services;

internal interface Deserializer<out T>
{
	T Deserialize(ref MemoryPackReader reader);
}