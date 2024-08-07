using System.Collections.Immutable;
using FlakeId;
using MemoryPack;

namespace SightKeeper.Data.Binary.DataSets.Poser2D;

[MemoryPackable]
internal partial class Poser2DTag : Tag
{
	public ImmutableArray<Tag> KeyPoints { get; }
	public ImmutableArray<Poser.NumericItemProperty> Properties { get; }

	[MemoryPackConstructor]
	public Poser2DTag(
		Id id,
		string name,
		uint color,
		ImmutableArray<Tag> keyPoints,
		ImmutableArray<Poser.NumericItemProperty> properties)
		: base(id, name, color)
	{
		KeyPoints = keyPoints;
		Properties = properties;
	}

	public Poser2DTag(Id id, Domain.Model.DataSets.Tags.Tag tag, ImmutableArray<Tag> keyPoints, ImmutableArray<Poser.NumericItemProperty> properties) : base(id, tag)
	{
		KeyPoints = keyPoints;
		Properties = properties;
	}
}