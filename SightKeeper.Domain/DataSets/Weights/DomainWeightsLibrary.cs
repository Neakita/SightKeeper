using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class DomainWeightsLibrary : WeightsLibrary
{
	public IReadOnlyCollection<Weights> Weights => _inner.Weights;

	public void CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		ValidateTags(tags);
		_inner.CreateWeights(metadata, tags);
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
			ValidateTagOwner(tagsList, tag);
	}

	private void ValidateTagOwner(IReadOnlyCollection<Tag> tagsList, Tag tag)
	{
		if (TryValidateAsKeyPointTag(tagsList, tag))
			return;
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(_tagsOwner, tag);
	}

	private static bool TryValidateAsKeyPointTag(IReadOnlyCollection<Tag> tagsList, Tag tag)
	{
		if (tag.Owner is not PoserTag poserTag)
			return false;
		if (!tagsList.Contains(poserTag))
			throw new KeyPointTagWithoutOwnerException(tag, poserTag);
		return true;

	}
}