using System.Buffers;
using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets;

internal static class DataSetGeneralDataFormatter
{
	public static void WriteGeneralData<TBufferWriter>(
		ref MemoryPackWriter<TBufferWriter> writer,
		DataSet<Tag, Asset> set)
		where TBufferWriter : IBufferWriter<byte>
	{
		writer.WriteString(set.Name);
		writer.WriteString(set.Description);
	}

	public static void ReadGeneralData(ref MemoryPackReader reader, DataSet<Tag, Asset> dataSet)
	{
		var name = reader.ReadString();
		Guard.IsNotNull(name);
		var description = reader.ReadString();
		Guard.IsNotNull(description);
		dataSet.Name = name;
		dataSet.Description = description;
	}
}