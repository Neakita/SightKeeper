using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Services;

namespace SightKeeper.Domain.Tests;

public sealed class SimplePoserWeightsDataAccess : PoserWeightsDataAccess
{
	public override WeightsData LoadWeightsData(PoserWeights weights)
	{
		return _data[weights];
	}

	protected override void SaveWeightsData(PoserWeights weights, WeightsData data)
	{
		_data.Add(weights, data);
	}

	protected override void RemoveWeightsData(PoserWeights weights)
	{
		Guard.IsTrue(_data.Remove(weights));
	}

	private readonly Dictionary<PoserWeights, WeightsData> _data = new();
}