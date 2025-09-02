namespace SightKeeper.Domain.DataSets.Assets.Items;

public sealed class ItemBoundingConstraintException : Exception
{
	public static void ThrowIfNotNormalized(BoundedItem item, Bounding value)
	{
		if (!IsNormalized(value))
			ThrowForNotNormalized(item, value);
	}

	private static void ThrowForNotNormalized(BoundedItem item, Bounding value)
	{
		const string message =
			"Bounding must be normalized, i.e. it must not have side coordinates less than 0 or greater than 1";
		throw new ItemBoundingConstraintException(message, item, value);
	}

	public BoundedItem Item { get; }
	public Bounding Value { get; }

	public ItemBoundingConstraintException(string? message, BoundedItem item, Bounding value) : base(message)
	{
		Item = item;
		Value = value;
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