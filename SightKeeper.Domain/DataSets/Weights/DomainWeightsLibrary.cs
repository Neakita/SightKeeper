using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class DomainWeightsLibrary : WeightsLibrary
{
	public IReadOnlyCollection<Weights> Weights => _inner.Weights;

	public void AddWeights(Weights weights)
	{
		ValidateTags(weights.Tags);
		_inner.AddWeights(weights);
	}

	public void RemoveWeights(Weights weights)
	{
		_inner.RemoveWeights(weights);
	}

	internal DomainWeightsLibrary(WeightsLibrary inner, TagsContainer<Tag> tagsOwner, int minimumTagsCount = 1)
	{
		_inner = inner;
		_minimumTagsCount = minimumTagsCount;
		_tagsOwner = tagsOwner;
	}

	private readonly WeightsLibrary _inner;
	private readonly int _minimumTagsCount;
	private readonly TagsContainer<Tag> _tagsOwner;

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
			UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(_tagsOwner, tag);
			if (tag.Owner is not DomainPoserTag poserTag)
				continue;
			if (!tagsList.Contains(poserTag))
				throw new KeyPointTagWithoutOwnerException(tag, poserTag);
		}
	}
}