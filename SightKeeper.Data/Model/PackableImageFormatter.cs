using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain;

namespace SightKeeper.Data.Model;

internal sealed class PackableImageFormatter : MemoryPackFormatter<PackableImage>
{
	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref PackableImage? value)
	{
		if (value == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		writer.WriteUnmanagedWithObjectHeader(3, value.Id, value.CreationTimestamp, value.Size);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref PackableImage? value)
	{
		if (!reader.TryReadObjectHeader(out var count))
        {
            value = null;
            return;
        }
        if (count != 3)
	        MemoryPackSerializationException.ThrowInvalidPropertyCount(typeof(PackableImage), 3, count);
        Guard.IsNull(value);
        reader.ReadUnmanaged(out Id id, out DateTimeOffset creationTimestamp, out Vector2<ushort> size);
        value = new PackableImage(creationTimestamp, size, id);
	}
}