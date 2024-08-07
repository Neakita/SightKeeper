using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Data.Binary.DataSets.Detector;

[MemoryPackable]
internal partial class DetectorItem
{
	public Id TagId { get; }
	public Bounding Bounding { get; }

	public DetectorItem(Id tagId, Bounding bounding)
	{
		TagId = tagId;
		Bounding = bounding;
	}
}