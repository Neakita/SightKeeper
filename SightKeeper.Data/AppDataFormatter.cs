﻿using MemoryPack;
using Serilog;
using SightKeeper.Application;
using SightKeeper.Data.Conversion;
using SightKeeper.Data.Replication;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

public sealed class AppDataFormatter : MemoryPackFormatter<AppData>
{
	public AppDataFormatter(IdRepository<Image> imageRepository, [Tag(typeof(AppData))] Lock editingLock, ILogger logger)
	{
		_editingLock = editingLock;
		_converter = new AppDataConverter(imageRepository);
		_replicator = new AppDataReplicator(imageRepository, logger.ForContext<AppDataReplicator>());
	}

	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref AppData? value)
	{
		if (value == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		PackableAppData packable;
		lock (_editingLock)
			packable = _converter.Convert(value);
		writer.WritePackable(packable);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref AppData? value)
	{
		if (reader.PeekIsNull())
		{
			reader.Advance(1); // skip null block
			value = null;
			return;
		}
		var packable = reader.ReadPackable<PackableAppData>();
		if (packable == null)
		{
			value = null;
			return;
		}
		value = _replicator.Replicate(packable);
	}

	private readonly Lock _editingLock;
	private readonly AppDataConverter _converter;
	private readonly AppDataReplicator _replicator;
}