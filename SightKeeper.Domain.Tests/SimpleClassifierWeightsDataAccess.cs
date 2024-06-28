using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Services;

namespace SightKeeper.Domain.Tests;

public sealed class SimpleClassifierWeightsDataAccess : ClassifierWeightsDataAccess
{
	public override WeightsData LoadWeightsData(ClassifierWeights weights)
	{
		return _data[weights];
	}

	protected override void SaveWeightsData(ClassifierWeights weights, WeightsData data)
	{
		_data.Add(weights, data);
	}

	protected override void RemoveWeightsData(ClassifierWeights weights)
	{
		Guard.IsTrue(_data.Remove(weights));
	}

	private readonly Dictionary<ClassifierWeights, WeightsData> _data = new();
}