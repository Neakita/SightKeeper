using System.Buffers;
using MemoryPack;
using SightKeeper.Application.Misc;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

internal sealed class MemoryPackDataSaver(
	Lock editingLock,
	Serializer<IReadOnlyCollection<ImageSet>> imageSetsSerializer,
	ReadRepository<ImageSet> imageSetsRepository,
	Serializer<IReadOnlyCollection<DataSet<Tag, Asset>>> dataSetsSerializer,
	ReadRepository<DataSet<Tag, Asset>> dataSetsRepository) :
	DataSaver
{
	private const ushort SchemaVersion = 3;

	public void Save()
	{
		var bytes = Serialize();
		if (File.Exists(PersistencePaths.FilePath))
			File.Copy(PersistencePaths.FilePath, PersistencePaths.BackupFilePath, true);
		File.WriteAllBytes(PersistencePaths.FilePath, bytes);
	}

	private byte[] Serialize()
	{
		ArrayBufferWriter<byte> bufferWriter = new();
		using var state = MemoryPackWriterOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
		var writer = new MemoryPackWriter<ArrayBufferWriter<byte>>(ref bufferWriter, state);
		writer.WriteUnmanaged(SchemaVersion);
		lock (editingLock)
		{
			imageSetsSerializer.Serialize(ref writer, imageSetsRepository.Items);
			dataSetsSerializer.Serialize(ref writer, dataSetsRepository.Items);
		}
		writer.Flush();
		return bufferWriter.WrittenSpan.ToArray();
	}
}