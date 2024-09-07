using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public abstract class WeightsLibrary
{
	public abstract int Count { get; }
	public abstract DataSet DataSet { get; }
	public abstract IReadOnlyCollection<Weights> Weights { get; }
}

public sealed class WeightsLibrary<TTag> : WeightsLibrary where TTag : Tag, MinimumTagsCount
{
	public override int Count => _weights.Count;
	public override DataSet DataSet { get; }
	public override IReadOnlyCollection<Weights<TTag>> Weights => _weights;

	internal WeightsLibrary(DataSet dataSet)
	{
		DataSet = dataSet;
	}

	public Weights<TTag> CreateWeights(
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<TTag> tags)
	{
		Weights<TTag> weights = new(creationDate, modelSize, metrics, resolution, tags, this);
		Guard.IsTrue(_weights.Add(weights));
		return weights;
	}

	public void RemoveWeights(Weights<TTag> weights)
	{
		Guard.IsTrue(_weights.Remove(weights));
	}

	private readonly SortedSet<Weights<TTag>> _weights = new(WeightsDateComparer.Instance);
}

public sealed class WeightsLibrary<TTag, TKeyPointTag> : WeightsLibrary
	where TTag : PoserTag
	where TKeyPointTag : KeyPointTag<TTag>
{
	public override int Count => _weights.Count;
	public override DataSet DataSet { get; }
	public override IReadOnlyCollection<Weights<TTag, TKeyPointTag>> Weights => _weights;

	internal WeightsLibrary(DataSet dataSet)
	{
		DataSet = dataSet;
	}

	public Weights<TTag, TKeyPointTag> CreateWeights(
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<(TTag, IEnumerable<TKeyPointTag>)> tags)
	{
		Weights<TTag, TKeyPointTag> weights = new(creationDate, modelSize, metrics, resolution, tags, this);
		Guard.IsTrue(_weights.Add(weights));
		return weights;
	}

	public void RemoveWeights(Weights<TTag, TKeyPointTag> weights)
	{
		Guard.IsTrue(_weights.Remove(weights));
	}

	private readonly SortedSet<Weights<TTag, TKeyPointTag>> _weights = new(WeightsDateComparer.Instance);
}