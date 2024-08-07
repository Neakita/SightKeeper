using FlakeId;
using MemoryPack;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
internal partial class Screenshot
{
	public Id Id { get; }
	public DateTime CreationDate { get; }

	public Screenshot(Id id, DateTime creationDate)
	{
		Id = id;
		CreationDate = creationDate;
	}
}