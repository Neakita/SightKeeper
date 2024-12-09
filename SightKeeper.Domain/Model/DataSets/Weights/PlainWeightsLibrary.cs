using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public sealed class PlainWeightsLibrary : WeightsLibrary
{
	public override IReadOnlyCollection<PlainWeights> Weights => _weights;

	public PlainWeights CreateWeights(
		DateTimeOffset creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		Composition? composition,
		IEnumerable<Tag> tags)
	{
		tags = tags.ToList().AsReadOnly();
		DuplicateTagsException.ThrowIfContainsDuplicates(tags);
		PlainWeights weights = new(creationDate, modelSize, metrics, resolution, composition, tags.ToList().AsReadOnly());
		Guard.IsGreaterThanOrEqualTo(weights.Tags.Count, _minimumTagsCount);
		foreach (var tag in weights.Tags)
			Guard.IsReferenceEqualTo(tag.Owner, _tagsOwner);
		var isAdded = _weights.Add(weights);
		Guard.IsTrue(isAdded);
		return weights;
	}

	public void RemoveWeights(PlainWeights weights)
	{
		var isRemoved = _weights.Remove(weights);
		Guard.IsTrue(isRemoved);
	}

	internal PlainWeightsLibrary(int minimumTagsCount, TagsOwner tagsOwner)
	{
		_minimumTagsCount = minimumTagsCount;
		_tagsOwner = tagsOwner;
	}

	private readonly int _minimumTagsCount;
	private readonly TagsOwner _tagsOwner;
	private readonly SortedSet<PlainWeights> _weights = new(WeightsDateComparer.Instance);
}