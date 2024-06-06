namespace SightKeeper.Domain.Model.DataSets;

public abstract class Asset
{
	public AssetUsage Usage { get; set; } = AssetUsage.Any;
	public Screenshot Screenshot { get; }

	protected Asset(Screenshot screenshot)
	{
		Screenshot = screenshot;
	}
}