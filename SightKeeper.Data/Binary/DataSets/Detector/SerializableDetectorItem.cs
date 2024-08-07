using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Data.Binary.DataSets.Detector;

[MemoryPackable]
internal partial class SerializableDetectorItem
{
	public Id TagId { get; }
	public Bounding Bounding { get; }

	public SerializableDetectorItem(Id tagId, Bounding bounding)
	{
		TagId = tagId;
		Bounding = bounding;
	}
}