using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Domain.Model.Tests;

internal sealed class SimpleWeightsDataAccess : WeightsDataAccess
{
	public override WeightsData LoadWeightsONNXData(Weights weights)
	{
		return _weightsData[weights].onnx;
	}

	public override WeightsData LoadWeightsPTData(Weights weights)
	{
		return _weightsData[weights].pt;
	}

	protected override void SaveWeightsData(Weights weights, WeightsData onnxData, WeightsData ptData)
	{
		_weightsData.Add(weights, (onnxData, ptData));
	}

	protected override void RemoveWeightsData(Weights weights)
	{
		_weightsData.Remove(weights);
	}

	private readonly Dictionary<Weights, (WeightsData onnx, WeightsData pt)> _weightsData = new();
}