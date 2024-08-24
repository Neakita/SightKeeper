using System.Collections;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public abstract class WeightsLibrary : IReadOnlyCollection<Weights>
{
	public abstract int Count { get; }
	public abstract DataSet DataSet { get; }

	public abstract IEnumerator<Weights> GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}

public sealed class WeightsLibrary<TTag> : WeightsLibrary, IReadOnlyCollection<Weights<TTag>> where TTag : Tag, MinimumTagsCount
{
	public override int Count => _weights.Count;
	public override DataSet DataSet { get; }

	public override IEnumerator<Weights<TTag>> GetEnumerator()
	{
		return _weights.GetEnumerator();
	}

	internal WeightsLibrary(DataSet dataSet)
	{
		DataSet = dataSet;
	}

	public Weights<TTag> CreateWeights(
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IEnumerable<TTag> tags)
	{
		Weights<TTag> weights = new(creationDate, modelSize, metrics, tags, this);
		Guard.IsTrue(_weights.Add(weights));
		return weights;
	}

	public void RemoveWeights(Weights<TTag> weights)
	{
		Guard.IsTrue(_weights.Remove(weights));
	}

	private readonly SortedSet<Weights<TTag>> _weights = new(WeightsDateComparer.Instance);

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}

public sealed class WeightsLibrary<TTag, TKeyPointTag> : WeightsLibrary, IReadOnlyCollection<Weights<TTag, TKeyPointTag>>
	where TTag : PoserTag
	where TKeyPointTag : KeyPointTag<TTag>
{
	public override int Count => _weights.Count;
	public override DataSet DataSet { get; }

	public override IEnumerator<Weights<TTag, TKeyPointTag>> GetEnumerator()
	{
		return _weights.GetEnumerator();
	}

	internal WeightsLibrary(DataSet dataSet)
	{
		DataSet = dataSet;
	}

	public Weights<TTag, TKeyPointTag> CreateWeights(
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IEnumerable<(TTag, IEnumerable<TKeyPointTag>)> tags)
	{
		Weights<TTag, TKeyPointTag> weights = new(creationDate, modelSize, metrics, tags, this);
		Guard.IsTrue(_weights.Add(weights));
		return weights;
	}

	public void RemoveWeights(Weights<TTag, TKeyPointTag> weights)
	{
		Guard.IsTrue(_weights.Remove(weights));
	}

	private readonly SortedSet<Weights<TTag, TKeyPointTag>> _weights = new(WeightsDateComparer.Instance);

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}