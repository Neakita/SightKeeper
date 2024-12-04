using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public sealed class PoserWeightsLibrary : WeightsLibrary
{
	public override IReadOnlyCollection<PoserWeights> Weights => _weights;

	public PoserWeights CreateWeights(
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> tags,
		Composition? composition)
	{
		tags = tags.ToImmutableDictionary();
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