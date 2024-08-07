using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.Binary.Conversion;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.DataSets.Poser3D;

[MemoryPackable]
internal sealed partial class Poser3DItem
{
	public static Poser3DItem Create(Domain.Model.DataSets.Poser3D.Poser3DItem item, ConversionSession session)
	{
		return new Poser3DItem(
			session.Tags[item.Tag],
			item.Bounding,
			Convert(item.KeyPoints),
			item.NumericProperties,
			item.BooleanProperties);
	}

	private static ImmutableArray<Poser3DKeyPoint> Convert(IEnumerable<KeyPoint3D> keyPoints)
	{
		return keyPoints.Select(Poser3DKeyPoint.Create).ToImmutableArray();
	}
	
	public Id TagId { get; }
	public Bounding Bounding { get; }
	public ImmutableArray<Poser3DKeyPoint> KeyPoints { get; }
	public ImmutableList<double> NumericProperties { get; }
	public ImmutableList<bool> BooleanProperties { get; }

	public Poser3DItem(
		Id tagId,
		Bounding bounding,
		ImmutableArray<Poser3DKeyPoint> keyPoints,
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