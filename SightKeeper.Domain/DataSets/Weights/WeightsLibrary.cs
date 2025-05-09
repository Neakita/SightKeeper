using System.Collections.ObjectModel;
using System.Diagnostics;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class WeightsLibrary
{
	public IReadOnlyCollection<Weights> Weights => _weights;

	public Weights CreateWeights(
		Model model,
		DateTimeOffset creationTimestamp,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		ImageComposition? composition,
		params IEnumerable<Tag> tags)
	{
		var tagsList = tags.ToList().AsReadOnly();
		ValidateTags(tagsList, nameof(tags));
		Weights weights = new()
		{
			Model = model,
			CreationTimestamp = creationTimestamp,
			ModelSize = modelSize,
			Metrics = metrics,
			Resolution = resolution,
			Composition = composition,
			Tags = tagsList
		};
		var isAdded = _weights.Add(weights);
		Debug.Assert(isAdded);
		return weights;
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

	private void ValidateTags(ReadOnlyCollection<Tag> tagsList, string paramName)
	{
		DuplicateTagsException.ThrowIfContainsDuplicates(tagsList);
		ValidateTagsQuantity(tagsList, paramName);
		ValidateTagOwners(tagsList);
	}

	private void ValidateTagsQuantity(ReadOnlyCollection<Tag> tagsList, string paramName)
	{
		if (tagsList.Count < _minimumTagsCount)
			throw new ArgumentException($"Should specify at least {_minimumTagsCount} tags", paramName);
	}

	private void ValidateTagOwners(ReadOnlyCollection<Tag> tagsList)
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