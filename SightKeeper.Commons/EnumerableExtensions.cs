using CommunityToolkit.Diagnostics;

namespace SightKeeper.Commons;

public static class EnumerableExtensions
{
	public static float Median(this IEnumerable<float> values)
	{
		var sortedValues = values.Order().ToList();
		Guard.IsNotEmpty(sortedValues);
		var middle = sortedValues.Count / 2;
		if (sortedValues.Count % 2 == 0)
			return (sortedValues[middle - 1] + sortedValues[middle]) / 2;
		return sortedValues[middle];
	}
}