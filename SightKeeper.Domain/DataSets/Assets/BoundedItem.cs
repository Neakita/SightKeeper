namespace SightKeeper.Domain.DataSets.Assets;

public abstract class BoundedItem
{
	public Bounding Bounding
	{
		get;
		set
		{
			if (!IsNormalized(value))
			{
				const string boundingConstraintExceptionMessage =
					"Bounding must be normalized, i.e. it must not have side coordinates less than 0 or greater than 1";
				throw new ItemBoundingConstraintException(boundingConstraintExceptionMessage, this, value);
			}
			field = value;
		}
	}

	protected BoundedItem(Bounding bounding)
	{
		Bounding = bounding;
	}

	private static bool IsNormalized(Bounding bounding)
	{
		return IsNormalized(bounding.Left) &&
		       IsNormalized(bounding.Top) &&
		       IsNormalized(bounding.Right) &&
		       IsNormalized(bounding.Bottom);
	}

	private static bool IsNormalized(double value)
	{
		return value is >= 0 and <= 1;
	}
}