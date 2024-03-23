using System.Collections.Generic;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.Misc;

internal sealed class FakeWeightsDataAccess : WeightsDataAccess
{
	public override WeightsData LoadWeightsONNXData(Weights weights)
	{
		return _weights[weights].onnx;
	}

	public override WeightsData LoadWeightsPTData(Weights weights)
	{
		return _weights[weights].pt;
	}

	protected override void SaveWeightsData(Weights weights, WeightsData onnxData, WeightsData ptData)
	{
		_weights.Add(weights, (onnxData, ptData));
	}

	protected override void RemoveWeightsData(Weights weights)
	{
		_weights.Remove(weights);
	}

	private readonly Dictionary<Weights, (WeightsData onnx, WeightsData pt)> _weights = new();
}