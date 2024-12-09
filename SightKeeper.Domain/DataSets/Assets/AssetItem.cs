namespace SightKeeper.Domain.DataSets.Assets;

public abstract class AssetItem
{
	public Bounding Bounding
	{
		get;
		set
		{
			if (!value.IsNormalized())
			{
				const string boundingConstraintExceptionMessage =
					"Bounding must be normalized, i.e. it must not have side coordinates less than 0 or greater than 1";
				throw new ItemBoundingConstraintException(boundingConstraintExceptionMessage, this, value);
			}
			field = value;
		}
	}

	protected AssetItem(Bounding bounding)
	{
		Bounding = bounding;
	}
}