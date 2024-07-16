using System.Runtime.CompilerServices;
using FlakeId;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Binary.Services;

public sealed class FileSystemClassifierWeightsDataAccess : ClassifierWeightsDataAccess
{
	public string DirectoryPath
	{
		get => _dataAccess.DirectoryPath;
		set => _dataAccess.DirectoryPath = value;
	}

	public override WeightsData LoadWeightsData(ClassifierWeights weights)
	{
		var data = _dataAccess.ReadAllBytes(weights);
		return CreateWeightsData(data);

		[UnsafeAccessor(UnsafeAccessorKind.Constructor)]
		static extern WeightsData CreateWeightsData(byte[] content);
	}

	public Id GetId(ClassifierWeights weights)
	{
		return _dataAccess.GetId(weights);
	}

	public void AssociateId(ClassifierWeights weights, Id id)
	{
		_dataAccess.AssociateId(weights, id);
	}

	protected override void SaveWeightsData(ClassifierWeights weights, WeightsData data)
	{
		_dataAccess.WriteAllBytes(weights, data.Content);
	}

	protected override void RemoveWeightsData(ClassifierWeights weights)
	{
		_dataAccess.Delete(weights);
	}

	private readonly FileSystemDataAccess<ClassifierWeights> _dataAccess = new(".pt");
}