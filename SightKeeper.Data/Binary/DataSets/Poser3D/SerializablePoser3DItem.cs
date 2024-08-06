using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.Binary.Conversion;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.DataSets.Poser3D;

[MemoryPackable]
internal sealed partial class SerializablePoser3DItem
{
	public static SerializablePoser3DItem Create(Poser3DItem item, ConversionSession session)
	{
		return new SerializablePoser3DItem(
			session.Tags[item.Tag],
			item.Bounding,
			Convert(item.KeyPoints),
			item.NumericProperties,
			item.BooleanProperties);
	}

	private static ImmutableArray<SerializablePoser3DKeyPoint> Convert(IEnumerable<KeyPoint3D> keyPoints)
	{
		return keyPoints.Select(SerializablePoser3DKeyPoint.Create).ToImmutableArray();
	}
	
	public Id TagId { get; }
	public Bounding Bounding { get; }
	public ImmutableArray<SerializablePoser3DKeyPoint> KeyPoints { get; }
	public ImmutableList<double> NumericProperties { get; }
	public ImmutableList<bool> BooleanProperties { get; }

	public SerializablePoser3DItem(
		Id tagId,
		Bounding bounding,
		ImmutableArray<SerializablePoser3DKeyPoint> keyPoints,
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