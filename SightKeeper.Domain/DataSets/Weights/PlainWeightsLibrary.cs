using System.Collections.ObjectModel;
using System.Diagnostics;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class PlainWeightsLibrary : WeightsLibrary
{
	public override IReadOnlyCollection<PlainWeights> Weights => _weights;

	public PlainWeights CreateWeights(
		DateTimeOffset creationTimestamp,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		ImageComposition? composition,
		IEnumerable<Tag> tags)
	{
		var tagsList = tags.ToList().AsReadOnly();
		ValidateTags(tagsList, nameof(tags));
		PlainWeights weights = new(creationTimestamp, modelSize, metrics, resolution, composition, tagsList);
		var isAdded = _weights.Add(weights);
		Debug.Assert(isAdded);
		return weights;
	}

	public void RemoveWeights(PlainWeights weights)
	{
		var isRemoved = _weights.Remove(weights);
		if (!isRemoved)
			throw new ArgumentException("Specified weights was not found and therefore not deleted");
	}

	internal PlainWeightsLibrary(int minimumTagsCount, TagsOwner tagsOwner)
	{
		_minimumTagsCount = minimumTagsCount;
		_tagsOwner = tagsOwner;
	}

	private readonly int _minimumTagsCount;
	private readonly TagsOwner _tagsOwner;
	private readonly SortedSet<PlainWeights> _weights = new(WeightsDateComparer.Instance);

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
			UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(_tagsOwner, tag);
	}
}