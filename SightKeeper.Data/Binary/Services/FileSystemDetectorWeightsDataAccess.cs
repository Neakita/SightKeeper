using System.Runtime.CompilerServices;
using FlakeId;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Binary.Services;

public sealed class FileSystemDetectorWeightsDataAccess: DetectorWeightsDataAccess
{
	public string DirectoryPath
	{
		get => _dataAccess.DirectoryPath;
		set => _dataAccess.DirectoryPath = value;
	}

	public override WeightsData LoadWeightsData(DetectorWeights weights)
	{
		var data = _dataAccess.ReadAllBytes(weights);
		return CreateWeightsData(data);

		[UnsafeAccessor(UnsafeAccessorKind.Constructor)]
		static extern WeightsData CreateWeightsData(byte[] content);
	}

	public Id GetId(DetectorWeights weights)
	{
		return _dataAccess.GetId(weights);
	}

	public void AssociateId(DetectorWeights weights, Id id)
	{
		_dataAccess.AssociateId(weights, id);
	}

	protected override void SaveWeightsData(DetectorWeights weights, WeightsData data)
	{
		_dataAccess.WriteAllBytes(weights, data.Content);
	}

	protected override void RemoveWeightsData(DetectorWeights weights)
	{
		_dataAccess.Delete(weights);
	}

	private readonly FileSystemDataAccess<DetectorWeights> _dataAccess = new(".pt");
}