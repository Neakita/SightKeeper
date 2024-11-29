namespace SightKeeper.Domain.Model.DataSets.Assets;

public abstract class AssetItem
{
	public Bounding Bounding
	{
		get => field;
		set
		{
			Bounding.EnsureNormalized();
			field = value;
		}
	}

	protected AssetItem(Bounding bounding)
	{
		Bounding = bounding;
	}
}