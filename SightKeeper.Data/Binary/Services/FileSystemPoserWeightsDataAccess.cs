using System.Runtime.CompilerServices;
using FlakeId;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Binary.Services;

public sealed class FileSystemPoserWeightsDataAccess : PoserWeightsDataAccess
{
	public string DirectoryPath
	{
		get => _dataAccess.DirectoryPath;
		set => _dataAccess.DirectoryPath = value;
	}

	public override WeightsData LoadWeightsData(PoserWeights weights)
	{
		var data = _dataAccess.ReadAllBytes(weights);
		return CreateWeightsData(data);

		[UnsafeAccessor(UnsafeAccessorKind.Constructor)]
		static extern WeightsData CreateWeightsData(byte[] content);
	}

	public Id GetId(PoserWeights weights)
	{
		return _dataAccess.GetId(weights);
	}

	public void AssociateId(PoserWeights weights, Id id)
	{
		_dataAccess.AssociateId(weights, id);
	}

	protected override void SaveWeightsData(PoserWeights weights, WeightsData data)
	{
		_dataAccess.WriteAllBytes(weights, data.Content);
	}

	protected override void RemoveWeightsData(PoserWeights weights)
	{
		_dataAccess.Delete(weights);
	}

	private readonly FileSystemDataAccess<PoserWeights> _dataAccess = new(".pt");
}