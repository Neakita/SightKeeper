using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="Poser3DItem"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser3DItem
{
	public byte TagId { get; }
	public Bounding Bounding { get; }
	public ImmutableArray<PackableKeyPoint3D> KeyPoints { get; }
	public ImmutableList<double> NumericProperties { get; }
	public ImmutableList<bool> BooleanProperties { get; }

	public PackablePoser3DItem(
		byte tagId,
		Bounding bounding,
		ImmutableArray<PackableKeyPoint3D> keyPoints,
		ImmutableList<double> numericProperties,
		ImmutableList<bool> booleanProperties)
	{
		TagId = tagId;
		Bounding = bounding;
		KeyPoints = keyPoints;
		NumericProperties = numericProperties;
		BooleanProperties = booleanProperties;
	}
}