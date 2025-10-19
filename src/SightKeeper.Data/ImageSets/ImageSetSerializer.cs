using System.Buffers;
using MemoryPack;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal sealed class ImageSetSerializer(Serializer<IReadOnlyCollection<ManagedImage>> imagesSerializer) : Serializer<ImageSet>
{
	public void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, ImageSet set)
		where TBufferWriter : IBufferWriter<byte>
	{
		WriteGeneralInfo(ref writer, set);
		imagesSerializer.Serialize(ref writer, set.Images);
	}

	private static void WriteGeneralInfo<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, ImageSet set)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteString(set.Name);
		writer.WriteString(set.Description);
	}
}