using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Services;

namespace SightKeeper.Domain.Tests.DataSets;

public class SimpleWeightsDataAccess : WeightsDataAccess
{
	public override WeightsData LoadWeightsData(Weights weights)
	{
		return _data[weights];
	}

	protected override void SaveWeightsData(Weights weights, WeightsData data)
	{
		_data.Add(weights, data);
	}

	protected override void RemoveWeightsData(Weights weights)
	{
		Guard.IsTrue(_data.Remove(weights));
	}

	private readonly Dictionary<Weights, WeightsData> _data = new();
}