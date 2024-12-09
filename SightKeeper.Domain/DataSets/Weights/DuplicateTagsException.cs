using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class DuplicateTagsException : Exception
{
	internal static void ThrowIfContainsDuplicates(IEnumerable<Tag> tags)
	{
		HashSet<Tag> hashSet = new();
		var duplicates = tags.Where(tag => !hashSet.Add(tag)).ToList();
		if (duplicates.Count > 0)
			throw new DuplicateTagsException("Provided tags collection contains duplicates", duplicates);
	}

	public IReadOnlyCollection<Tag> Duplicates { get; }

	private DuplicateTagsException(string? message, IReadOnlyCollection<Tag> duplicates) : base(message)
	{
		Duplicates = duplicates;
	}
}