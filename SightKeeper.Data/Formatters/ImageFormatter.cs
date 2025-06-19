using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.Model.Images;
using SightKeeper.Domain;

namespace SightKeeper.Data.Formatters;

internal sealed class ImageFormatter : MemoryPackFormatter<InMemoryImage>
{
	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref InMemoryImage? value)
	{
		if (value == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		writer.WriteUnmanagedWithObjectHeader(3, value.Id, value.CreationTimestamp, value.Size);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref InMemoryImage? value)
	{
		if (!reader.TryReadObjectHeader(out var count))
        {
            value = null;
            return;
        }
        if (count != 3)
	        MemoryPackSerializationException.ThrowInvalidPropertyCount(typeof(InMemoryImage), 3, count);
        Guard.IsNull(value);
        reader.ReadUnmanaged(out Id id, out DateTimeOffset creationTimestamp, out Vector2<ushort> size);
        value = new InMemoryImage(creationTimestamp, size, id);
	}
}