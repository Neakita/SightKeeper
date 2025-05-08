using System.Collections.ObjectModel;
using System.Diagnostics;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class PoserWeightsLibrary : WeightsLibrary
{
	private const int MinimumTagsCount = 1;

	public override IReadOnlyCollection<PoserWeights> Weights => _weights;

	public PoserWeights CreateWeights(
		Model model,
		DateTimeOffset creationTimestamp,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		ImageComposition? composition,
		IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> tags)
	{
		tags = PreventExternalEditing(tags);
		ValidateTagsQuantity(tags);
		ValidateTagsOwners(tags);
		PoserWeights weights = new()
		{
			Model = model,
			CreationTimestamp = creationTimestamp,
			ModelSize = modelSize,
			Metrics = metrics,
			Resolution = resolution,
			Composition = composition,
			Tags = tags
		};
		var isAdded = _weights.Add(weights);
		Debug.Assert(isAdded);
		return weights;
	}

	public void RemoveWeights(PoserWeights weights)
	{
		var isRemoved = _weights.Remove(weights);
		if (!isRemoved)
			throw new ArgumentException("Specified weights was not found and therefore not deleted");
	}

	internal PoserWeightsLibrary(TagsContainer<Tag> tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	private readonly TagsContainer<Tag> _tagsOwner;
	private readonly SortedSet<PoserWeights> _weights = new(WeightsDateComparer.Instance);

	private static ReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> PreventExternalEditing(IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> tags)
	{
		return tags
			.ToDictionary(
				pair => pair.Key,
				IReadOnlyCollection<Tag> (pair) => pair.Value.ToList().AsReadOnly())
			.AsReadOnly();
	}

	private static void ValidateTagsQuantity(IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> tags)
	{
		if (tags.Count < MinimumTagsCount)
			throw new ArgumentException($"Should specify at least {MinimumTagsCount} tags, but was {tags.Count}");
	}

	private void ValidateTagsOwners(IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> tags)
	{
		foreach (var (poserTag, keyPointTags) in tags)
		{
			UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(_tagsOwner, poserTag);
			foreach (var keyPointTag in keyPointTags)
				UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(poserTag, keyPointTag);
		}
	}
}