using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Poser;

[MemoryPackable]
internal sealed partial class SerializablePoserItem
{
	public Id TagId { get; }
	public Bounding Bounding { get; }
	public ImmutableArray<Vector2<double>> KeyPoints { get; }

	public SerializablePoserItem(Id tagId, Bounding bounding, ImmutableArray<Vector2<double>> keyPoints)
	{
		TagId = tagId;
		Bounding = bounding;
		KeyPoints = keyPoints;
	}
}