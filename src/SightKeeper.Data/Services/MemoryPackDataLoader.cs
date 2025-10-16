using MemoryPack;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

internal sealed class MemoryPackDataLoader(
	Deserializer<IEnumerable<ImageSet>> imageSetDeserializer,
	ShortcutWriteRepository<ImageSet> imageSetsRepository,
	Deserializer<IEnumerable<DataSet<Tag, Asset>>> dataSetDeserializer,
	ShortcutWriteRepository<DataSet<Tag, Asset>> dataSetsRepository) :
	DataLoader
{
	public void Load()
	{
		if (!File.Exists(PersistencePaths.FilePath))
			return;
		var bytes = File.ReadAllBytes(PersistencePaths.FilePath);
		Deserialize(bytes);
	}

	private void Deserialize(byte[] bytes)
	{
		using var state = MemoryPackReaderOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
		var reader = new MemoryPackReader(bytes, state);
		reader.ReadUnmanaged(out ushort schemaVersion);
		Deserialize(ref reader, imageSetDeserializer, imageSetsRepository);
		Deserialize(ref reader, dataSetDeserializer, dataSetsRepository);
	}

	private static void Deserialize<T>(
		ref MemoryPackReader reader,
		Deserializer<IEnumerable<T>> deserializer,
		ShortcutWriteRepository<T> repository)
	{
		var items = deserializer.Deserialize(ref reader);
		foreach (var item in items)
			repository.Add(item);
	}
}