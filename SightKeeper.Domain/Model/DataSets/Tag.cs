namespace SightKeeper.Domain.Model.DataSets;

public abstract class Tag
{
	public abstract string Name { get; set; }
	public uint Color { get; set; }
}