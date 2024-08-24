using FlakeId;
using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Services;

public sealed class FileSystemWeightsDataAccess: WeightsDataAccess
{
	public string DirectoryPath
	{
		get => _dataAccess.DirectoryPath;
		set => _dataAccess.DirectoryPath = value;
	}

	public override byte[] LoadWeightsData(Weights weights)
	{
		return _dataAccess.ReadAllBytes(weights);
	}

	public Id GetId(Weights weights)
	{
		return _dataAccess.GetId(weights);
	}

	public void AssociateId(Weights weights, Id id)
	{
		_dataAccess.AssociateId(weights, id);
	}

	protected override void SaveWeightsData(Weights weights, byte[] data)
	{
		_dataAccess.WriteAllBytes(weights, data);
	}

	protected override void RemoveWeightsData(Weights weights)
	{
		_dataAccess.Delete(weights);
	}

	private readonly FileSystemDataAccess<Weights> _dataAccess = new(".pt");
}