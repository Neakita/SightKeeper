using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets;

internal sealed class DataSetFormatter(IReadOnlyList<MemoryPackDataSetDeserializer> deserializersByUnionTag)
	: MemoryPackFormatter<DataSet<Tag, Asset>>
{
	public override void Serialize<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		scoped ref DataSet<Tag, Asset>? set)
	{
		if (set == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		var serializable = set.Get<MemoryPackSerializable>();
		serializable.Serialize(ref writer);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref DataSet<Tag, Asset>? set)
	{
		if (reader.PeekIsNull())
		{
			set = null;
			return;
		}
		Guard.IsTrue(reader.TryReadUnionHeader(out var unionTag));
		var deserializer = deserializersByUnionTag[unionTag];
		set = deserializer.Deserialize(ref reader);
	}
}