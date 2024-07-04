using FlakeId;
using MemoryPack;

namespace SightKeeper.Data.Binary.Formatters;

public sealed class IdFormatter : MemoryPackFormatter<Id>
{
	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref Id value)
	{
		writer.WriteVarInt(value);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref Id value)
	{
		value = new Id(reader.ReadVarIntInt64());
	}
}