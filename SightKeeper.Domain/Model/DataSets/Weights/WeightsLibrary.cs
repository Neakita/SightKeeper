using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public abstract class WeightsLibrary
{
	public abstract DataSet DataSet { get; }
	public abstract IReadOnlyCollection<Weights> Weights { get; }
}

public sealed class WeightsLibrary<TTag> : WeightsLibrary where TTag : Tag, MinimumTagsCount
{
	public override DataSet DataSet { get; }
	public override IReadOnlyCollection<PlainWeights<TTag>> Weights => _weights;

	internal WeightsLibrary(DataSet dataSet)
	{
		DataSet = dataSet;
	}

	public PlainWeights<TTag> CreateWeights(
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<TTag> tags)
	{
		PlainWeights<TTag> weights = new(creationDate, modelSize, metrics, resolution, tags, this);
		Guard.IsTrue(_weights.Add(weights));
		return weights;
	}

	public void RemoveWeights(PlainWeights<TTag> weights)
	{
		Guard.IsTrue(_weights.Remove(weights));
	}

	private readonly SortedSet<PlainWeights<TTag>> _weights = new(WeightsDateComparer.Instance);
}

public sealed class WeightsLibrary<TTag, TKeyPointTag> : WeightsLibrary
	where TTag : PoserTag
	where TKeyPointTag : KeyPointTag<TTag>
{
	public override DataSet DataSet { get; }
	public override IReadOnlyCollection<PoserWeights<TTag, TKeyPointTag>> Weights => _weights;

	internal WeightsLibrary(DataSet dataSet)
	{
		DataSet = dataSet;
	}

	public PoserWeights<TTag, TKeyPointTag> CreateWeights(
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<(TTag, IEnumerable<TKeyPointTag>)> tags)
	{
		PoserWeights<TTag, TKeyPointTag> weights = new(creationDate, modelSize, metrics, resolution, tags, this);
		Guard.IsTrue(_weights.Add(weights));
		return weights;
	}

	public void RemoveWeights(PoserWeights<TTag, TKeyPointTag> weights)
	{
		Guard.IsTrue(_weights.Remove(weights));
	}

	private readonly SortedSet<PoserWeights<TTag, TKeyPointTag>> _weights = new(WeightsDateComparer.Instance);
}