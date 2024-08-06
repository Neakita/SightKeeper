using MemoryPack;

namespace SightKeeper.Data.Binary.DataSets.Poser2D;

[MemoryPackable]
internal sealed partial class SerializableNumericItemProperty
{
	public double MinimumValue { get; set; }
	public double MaximumValue { get; set; }
}