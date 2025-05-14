using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class WeightsLibrary
{
	public IReadOnlyCollection<Weights> Weights => _weights;

	public void AddWeights(Weights weights)
	{
		ValidateTags(weights.Tags);
		var isAdded = _weights.Add(weights);
		if (!isAdded)
			throw new ArgumentException("Specified weights already exists in the library");
	}

	public void RemoveWeights(Weights weights)
	{
		var isRemoved = _weights.Remove(weights);
		if (!isRemoved)
			throw new ArgumentException("Specified weights was not found and therefore not deleted");
	}

	internal WeightsLibrary(TagsContainer<Tag> tagsOwner, int minimumTagsCount = 1)
	{
		_minimumTagsCount = minimumTagsCount;
		_tagsOwner = tagsOwner;
	}

	private readonly int _minimumTagsCount;
	private readonly TagsContainer<Tag> _tagsOwner;
	private readonly SortedSet<Weights> _weights = new(WeightsDateComparer.Instance);

	private void ValidateTags(IReadOnlyCollection<Tag> tagsList)
	{
		DuplicateTagsException.ThrowIfContainsDuplicates(tagsList);
		ValidateTagsQuantity(tagsList);
		ValidateTagOwners(tagsList);
	}

	private void ValidateTagsQuantity(IReadOnlyCollection<Tag> tagsList)
	{
		if (tagsList.Count < _minimumTagsCount)
			throw new ArgumentException($"Should specify at least {_minimumTagsCount} tags");
	}

	private void ValidateTagOwners(IReadOnlyCollection<Tag> tagsList)
	{
		foreach (var tag in tagsList)
		{
			if (tag.Owner is TagsLibrary)
				UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(_tagsOwner, tag);
			else if (tag.Owner is PoserTag poserTag)
			{
				if (!tagsList.Contains(poserTag))
					throw new KeyPointTagWithoutOwnerException(tag, poserTag);
			}
		}
	}
}