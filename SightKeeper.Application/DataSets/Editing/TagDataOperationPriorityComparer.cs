using CommunityToolkit.Diagnostics;
using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Editing;

internal sealed class TagDataOperationPriorityComparer : IComparer<TagData>
{
	public static TagDataOperationPriorityComparer Instance { get; } = new();

	public int Compare(TagData? x, TagData? y)
	{
		Guard.IsNotNull(x);
		Guard.IsNotNull(y);
		byte xPriority = CalculatePriority(x);
		byte yPriority = CalculatePriority(y);
		return xPriority.CompareTo(yPriority);
	}

	private static byte CalculatePriority(TagData data)
	{
		return data switch
		{
			ExistingTagData and NewTagData => 1,
			ExistingTagData => 0,
			NewTagData => 2,
			_ => throw new ArgumentOutOfRangeException(nameof(data))
		};
	}
}