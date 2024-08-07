using System.Runtime.CompilerServices;
using FlakeId;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Binary.Services;

public sealed class FileSystemWeightsDataAccess: WeightsDataAccess
{
	public string DirectoryPath
	{
		get => _dataAccess.DirectoryPath;
		set => _dataAccess.DirectoryPath = value;
	}

	public override WeightsData LoadWeightsData(Weights weights)
	{
		var data = _dataAccess.ReadAllBytes(weights);
		return CreateWeightsData(data);

		[UnsafeAccessor(UnsafeAccessorKind.Constructor)]
		static extern WeightsData CreateWeightsData(byte[] content);
	}

	public Id GetId(Weights weights)
	{
		return _dataAccess.GetId(weights);
	}

	public void AssociateId(Weights weights, Id id)
	{
		_dataAccess.AssociateId(weights, id);
	}

	protected override void SaveWeightsData(Weights weights, WeightsData data)
	{
		_dataAccess.WriteAllBytes(weights, data.Content);
	}

	protected override void RemoveWeightsData(Weights weights)
	{
		_dataAccess.Delete(weights);
	}

	private readonly FileSystemDataAccess<Weights> _dataAccess = new(".pt");
}