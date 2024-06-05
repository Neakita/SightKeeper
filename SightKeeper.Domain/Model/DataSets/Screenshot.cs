namespace SightKeeper.Domain.Model.DataSets;

public sealed class Screenshot
{
	public DateTime CreationDate { get; } = DateTime.Now;

	internal Screenshot()
	{
	}
}