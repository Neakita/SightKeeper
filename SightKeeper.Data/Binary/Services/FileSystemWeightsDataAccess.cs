﻿using FlakeId;
using SightKeeper.Application;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Services;

public sealed class FileSystemWeightsDataAccess: WeightsDataAccess
{
	public string DirectoryPath
	{
		get => _weightsDataAccess.DirectoryPath;
		set => _weightsDataAccess.DirectoryPath = value;
	}

	public FileSystemWeightsDataAccess(AppDataAccess appDataAccess, AppDataEditingLock editingLock)
	{
		_appDataAccess = appDataAccess;
		_editingLock = editingLock;
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

	protected override PlainWeights CreateWeights(PlainWeightsLibrary library, DateTimeOffset creationDate, ModelSize modelSize, WeightsMetrics metrics, Vector2<ushort> resolution, IEnumerable<Tag> tags, Composition? composition)
	{
		lock (_editingLock)
			return base.CreateWeights(library, creationDate, modelSize, metrics, resolution, tags, composition);
	}

	protected override PoserWeights CreateWeights(PoserWeightsLibrary library, DateTimeOffset creationDate, ModelSize modelSize, WeightsMetrics metrics, Vector2<ushort> resolution, Composition? composition, IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> tags)
	{
		lock (_editingLock)
			return base.CreateWeights(library, creationDate, modelSize, metrics, resolution, composition, tags);
	}

	protected override void SaveWeightsData(Weights weights, byte[] data)
	{
		_weightsDataAccess.WriteAllBytes(weights, data);
	}

	protected override void RemoveWeights(PlainWeightsLibrary library, PlainWeights weights)
	{
		lock (_editingLock)
			base.RemoveWeights(library, weights);
		_appDataAccess.SetDataChanged();
	}

	protected override void RemoveWeights(PoserWeightsLibrary library, PoserWeights weights)
	{
		lock (_editingLock)
			base.RemoveWeights(library, weights);
		_appDataAccess.SetDataChanged();
	}

	protected override void RemoveWeightsData(Weights weights)
	{
		_weightsDataAccess.Delete(weights);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly AppDataEditingLock _editingLock;
	private readonly FileSystemDataAccess<Weights> _weightsDataAccess = new(".pt");
}