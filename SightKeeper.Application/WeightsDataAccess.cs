using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Application;

public abstract class WeightsDataAccess
{
	public Weights CreateWeights(
		WeightsLibrary library,
		byte[] data,
		Model model,
		DateTimeOffset creationTimestamp,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<Tag> tags,
		ImageComposition? composition)
	{
		var weights = CreateWeights(library, model, creationTimestamp, modelSize, metrics, resolution, tags, composition);
		SaveWeightsData(weights, data);
		return weights;
	}

	public void DeleteWeights(WeightsLibrary library, Weights weights)
	{
		RemoveWeights(library, weights);
		RemoveWeightsData(weights);
	}

	public abstract byte[] LoadWeightsData(Weights weights);

	protected virtual Weights CreateWeights(WeightsLibrary library, Model model, DateTimeOffset creationTimestamp, ModelSize modelSize, WeightsMetrics metrics, Vector2<ushort> resolution, IEnumerable<Tag> tags, ImageComposition? composition)
	{
		return library.CreateWeights(model, creationTimestamp, modelSize, metrics, resolution, composition, tags);
	}

	protected virtual void RemoveWeights(WeightsLibrary library, Weights weights)
	{
		library.RemoveWeights(weights);
	}

	protected abstract void SaveWeightsData(Weights weights, byte[] data);
	protected abstract void RemoveWeightsData(Weights weights);
}