using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Detector;

[MemoryPackable]
public partial class SerializableDetectorItem
{
	public Id TagId { get; }
	public Bounding Bounding { get; }

	public SerializableDetectorItem(Id tagId, Bounding bounding)
	{
		TagId = tagId;
		Bounding = bounding;
	}
}