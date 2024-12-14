using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class PoserWeightsLibrary : WeightsLibrary
{
	private const int MinimumTagsCount = 1;

	public override IReadOnlyCollection<PoserWeights> Weights => _weights;

	public PoserWeights CreateWeights(
		DateTimeOffset creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		ImageComposition? composition,
		IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> tags)
	{
		tags = tags
			.ToDictionary(
				pair => pair.Key,
				IReadOnlyCollection<Tag> (pair) => pair.Value.ToList().AsReadOnly())
			.AsReadOnly();
		Guard.IsGreaterThanOrEqualTo(tags.Count, MinimumTagsCount);
		foreach (var (poserTag, keyPointTags) in tags)
		{
			UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(_tagsOwner, poserTag);
			foreach (var keyPointTag in keyPointTags)
				UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(poserTag, keyPointTag);
		}
		PoserWeights weights = new(creationDate, modelSize, metrics, resolution, composition, tags);
		var isAdded = _weights.Add(weights);
		Guard.IsTrue(isAdded);
		return weights;
	}

	public void RemoveWeights(PoserWeights weights)
	{
		var isRemoved = _weights.Remove(weights);
		Guard.IsTrue(isRemoved);
	}

	internal PoserWeightsLibrary(TagsOwner tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	private readonly TagsOwner _tagsOwner;
	private readonly SortedSet<PoserWeights> _weights = new(WeightsDateComparer.Instance);
}