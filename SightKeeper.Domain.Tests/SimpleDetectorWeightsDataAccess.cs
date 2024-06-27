using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Domain.Tests;

public class SimpleDetectorWeightsDataAccess : DetectorWeightsDataAccess
{
	public override WeightsData LoadWeightsData(DetectorWeights weights)
	{
		return _data[weights];
	}

	protected override void SaveWeightsData(DetectorWeights weights, WeightsData data)
	{
		_data.Add(weights, data);
	}

	protected override void RemoveWeightsData(DetectorWeights weights)
	{
		Guard.IsTrue(_data.Remove(weights));
	}

	private readonly Dictionary<DetectorWeights, WeightsData> _data = new();
}