using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application;

public abstract class WeightsDataAccess
{
	public PlainWeights CreateWeights(
		PlainWeightsLibrary library,
		byte[] data,
		DateTimeOffset creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<Tag> tags,
		Composition? composition)
	{
		var weights = CreateWeights(library, creationDate, modelSize, metrics, resolution, tags, composition);
		SaveWeightsData(weights, data);
		return weights;
	}

	public PoserWeights CreateWeights(
		PoserWeightsLibrary library,
		byte[] data,
		DateTimeOffset creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		Composition? composition,
		IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> tags)
	{
		var weights = CreateWeights(library, creationDate, modelSize, metrics, resolution, composition, tags);
		SaveWeightsData(weights, data);
		return weights;
	}

	public void DeleteWeights(PlainWeightsLibrary library, PlainWeights weights)
	{
		RemoveWeights(library, weights);
		RemoveWeightsData(weights);
	}

	public void DeleteWeights(PoserWeightsLibrary library, PoserWeights weights)
	{
		RemoveWeights(library, weights);
		RemoveWeightsData(weights);
	}

	public abstract byte[] LoadWeightsData(Weights weights);

	protected virtual PlainWeights CreateWeights(PlainWeightsLibrary library, DateTimeOffset creationDate, ModelSize modelSize, WeightsMetrics metrics, Vector2<ushort> resolution, IEnumerable<Tag> tags, Composition? composition)
	{
		return library.CreateWeights(creationDate, modelSize, metrics, resolution, composition, tags);
	}

	protected virtual PoserWeights CreateWeights(PoserWeightsLibrary library, DateTimeOffset creationDate, ModelSize modelSize, WeightsMetrics metrics, Vector2<ushort> resolution, Composition? composition, IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> tags)
	{
		return library.CreateWeights(creationDate, modelSize, metrics, resolution, composition, tags);
	}

	protected virtual void RemoveWeights(PlainWeightsLibrary library, PlainWeights weights)
	{
		library.RemoveWeights(weights);
	}

	protected virtual void RemoveWeights(PoserWeightsLibrary library, PoserWeights weights)
	{
		library.RemoveWeights(weights);
	}

	protected abstract void SaveWeightsData(Weights weights, byte[] data);
	protected abstract void RemoveWeightsData(Weights weights);
}