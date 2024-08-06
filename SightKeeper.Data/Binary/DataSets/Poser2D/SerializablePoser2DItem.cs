using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Poser2D;

[MemoryPackable]
internal sealed partial class SerializablePoser2DItem
{
	public Id TagId { get; }
	public Bounding Bounding { get; }
	public ImmutableArray<Vector2<double>> KeyPoints { get; }
	public ImmutableList<double> Properties { get; }

	public SerializablePoser2DItem(Id tagId, Bounding bounding, ImmutableArray<Vector2<double>> keyPoints, ImmutableList<double> properties)
	{
		TagId = tagId;
		Bounding = bounding;
		KeyPoints = keyPoints;
		Properties = properties;
	}
}