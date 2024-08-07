using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.Binary.DataSets.Poser;

namespace SightKeeper.Data.Binary.DataSets.Poser3D;

[MemoryPackable]
internal sealed partial class Poser3DTag : Tag
{
	public ImmutableArray<Tag> KeyPoints { get; }
	public ImmutableArray<NumericItemProperty> NumericProperties { get; }
	public ImmutableArray<string> BooleanProperties { get; }

	[MemoryPackConstructor]
	public Poser3DTag(
		Id id,
		string name,
		uint color,
		ImmutableArray<Tag> keyPoints,
		ImmutableArray<NumericItemProperty> numericProperties,
		ImmutableArray<string> booleanProperties)
		: base(id, name, color)
	{
		KeyPoints = keyPoints;
		NumericProperties = numericProperties;
		BooleanProperties = booleanProperties;
	}

	public Poser3DTag(
		Id id,
		Domain.Model.DataSets.Tags.Tag tag,
		ImmutableArray<Tag> keyPoints,
		ImmutableArray<NumericItemProperty> numericProperties,
		ImmutableArray<string> booleanProperties) : base(id, tag)
	{
		KeyPoints = keyPoints;
		NumericProperties = numericProperties;
		BooleanProperties = booleanProperties;
	}
}