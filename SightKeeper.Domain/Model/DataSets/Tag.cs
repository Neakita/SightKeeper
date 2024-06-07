namespace SightKeeper.Domain.Model.DataSets;

public abstract class Tag
{
	public abstract string Name { get; set; }
	public abstract uint Color { get; set; }
}