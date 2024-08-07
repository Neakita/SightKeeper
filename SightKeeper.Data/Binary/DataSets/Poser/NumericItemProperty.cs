using MemoryPack;

namespace SightKeeper.Data.Binary.DataSets.Poser;

[MemoryPackable]
internal sealed partial class NumericItemProperty
{
	public static NumericItemProperty Create(Domain.Model.DataSets.Poser.NumericItemProperty property)
	{
		return new NumericItemProperty(property.Name, property.MinimumValue, property.MaximumValue);
	}

	public string Name { get; }
	public double MinimumValue { get; }
	public double MaximumValue { get; }

	public NumericItemProperty(string name, double minimumValue, double maximumValue)
	{
		Name = name;
		MinimumValue = minimumValue;
		MaximumValue = maximumValue;
	}
}