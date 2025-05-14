using FlakeId;
using SightKeeper.Application;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Services;

public sealed class FileSystemWeightsDataAccess : WeightsDataAccess
{
	public string DirectoryPath
	{
		get => _weightsDataAccess.DirectoryPath;
		set => _weightsDataAccess.DirectoryPath = value;
	}

	public FileSystemWeightsDataAccess(AppDataAccess appDataAccess, [Tag(typeof(AppData))] Lock appDataLock)
	{
		_appDataAccess = appDataAccess;
		_appDataLock = appDataLock;
	}

	public override byte[] LoadWeightsData(Weights weights)
	{
		return _weightsDataAccess.ReadAllBytes(weights);
	}

	public Id GetId(Weights weights)
	{
		return _weightsDataAccess.GetId(weights);
	}

	public void AssociateId(Weights weights, Id id)
	{
		_weightsDataAccess.AssociateId(weights, id);
	}

	protected override void AddWeights(WeightsLibrary library, Weights weights)
	{
		lock (_appDataLock)
			base.AddWeights(library, weights);
	}

	protected override void SaveWeightsData(Weights weights, byte[] data)
	{
		_weightsDataAccess.WriteAllBytes(weights, data);
	}

	protected override void RemoveWeights(WeightsLibrary library, Weights weights)
	{
		lock (_appDataLock)
			base.RemoveWeights(library, weights);
		_appDataAccess.SetDataChanged();
	}

	protected override void RemoveWeightsData(Weights weights)
	{
		_weightsDataAccess.Delete(weights);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly Lock _appDataLock;
	private readonly FileSystemDataAccess<Weights> _weightsDataAccess = new(".pt");
}