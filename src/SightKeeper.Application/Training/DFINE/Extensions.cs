using CommunityToolkit.Diagnostics;

namespace SightKeeper.Application.Training.DFINE;

internal static class Extensions
{
	public static void ReplaceAt(this IList<string> lines, int index, string oldValue, string newValue)
	{
		var line = lines[index];
		Guard.IsTrue(line.Contains(oldValue));
		var modifiedLine = line.Replace(oldValue, newValue);
		lines[index] = modifiedLine;
	}
}