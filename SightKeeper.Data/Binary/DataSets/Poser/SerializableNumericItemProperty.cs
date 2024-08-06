using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.DataSets.Poser;

[MemoryPackable]
internal sealed partial class SerializableNumericItemProperty
{
	public static SerializableNumericItemProperty Create(NumericItemProperty property)
	{
		return new SerializableNumericItemProperty(property.Name, property.MinimumValue, property.MaximumValue);
	}

	public string Name { get; }
	public double MinimumValue { get; }
	public double MaximumValue { get; }

	public SerializableNumericItemProperty(string name, double minimumValue, double maximumValue)
	{
		Name = name;
		MinimumValue = minimumValue;
		MaximumValue = maximumValue;
	}
}