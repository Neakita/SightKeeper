﻿using FlakeId;
using MemoryPack;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
internal partial class SerializableScreenshot
{
	public Id Id { get; }
	public DateTime CreationDate { get; }

	public SerializableScreenshot(Id id, DateTime creationDate)
	{
		Id = id;
		CreationDate = creationDate;
	}
}