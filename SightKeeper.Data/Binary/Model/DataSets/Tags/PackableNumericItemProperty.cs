using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Model.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="NumericItemProperty"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableNumericItemProperty : PackableItemProperty
{
	public double MinimumValue { get; }
	public double MaximumValue { get; }

	public PackableNumericItemProperty(string name, double minimumValue, double maximumValue) : base(name)
	{
		MinimumValue = minimumValue;
		MaximumValue = maximumValue;
	}
}