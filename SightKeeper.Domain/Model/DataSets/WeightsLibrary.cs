using System.Collections;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class WeightsLibrary<TWeights> : IReadOnlyCollection<TWeights> where TWeights : Weights
{
	public int Count => _weights.Count;

	public IEnumerator<TWeights> GetEnumerator()
	{
		return _weights.GetEnumerator();
	}

	protected void AddWeights(TWeights weights)
	{
		bool isAdded = _weights.Add(weights);
		Guard.IsTrue(isAdded);
	}

	internal void RemoveWeights(TWeights weights)
	{
		var isRemoved = _weights.Remove(weights);
		Guard.IsTrue(isRemoved);
	}

	private readonly SortedSet<TWeights> _weights = new(WeightsDateComparer.Instance);

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
	
}