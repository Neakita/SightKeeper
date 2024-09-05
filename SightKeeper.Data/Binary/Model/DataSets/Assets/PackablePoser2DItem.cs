using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="Poser2DItem"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser2DItem
{
	public byte TagId { get; }
	public Bounding Bounding { get; }
	public ImmutableArray<KeyPoint2D> KeyPoints { get; }

	public PackablePoser2DItem(byte tagId, Bounding bounding, ImmutableArray<KeyPoint2D> keyPoints)
	{
		TagId = tagId;
		Bounding = bounding;
		KeyPoints = keyPoints;
	}
}