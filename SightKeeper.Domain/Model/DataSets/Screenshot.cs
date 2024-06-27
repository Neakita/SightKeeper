namespace SightKeeper.Domain.Model.DataSets;

public abstract class Screenshot
{
	public DateTime CreationDate { get; } = DateTime.Now;
}