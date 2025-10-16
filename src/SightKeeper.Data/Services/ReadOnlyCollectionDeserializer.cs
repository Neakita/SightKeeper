using CommunityToolkit.Diagnostics;
using MemoryPack;

namespace SightKeeper.Data.Services;

internal sealed class ReadOnlyCollectionDeserializer<T>(Deserializer<T> itemDeserializer) : Deserializer<IReadOnlyCollection<T>>
{
	public IReadOnlyCollection<T> Deserialize(ref MemoryPackReader reader)
	{
		Guard.IsTrue(reader.TryReadCollectionHeader(out var imagesCount));
		var imagesArray = new T[imagesCount];
		for (int i = 0; i < imagesCount; i++)
		{
			var image = itemDeserializer.Deserialize(ref reader);
			imagesArray[i] = image;
		}
		return imagesArray;
	}
}