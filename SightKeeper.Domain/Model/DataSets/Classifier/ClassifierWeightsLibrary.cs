namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierWeightsLibrary : WeightsLibrary<ClassifierWeights>
{
	public ClassifierDataSet DataSet { get; }

	internal ClassifierWeightsLibrary(ClassifierDataSet dataSet)
	{
		DataSet = dataSet;
	}

	internal ClassifierWeights CreateWeights(
		ModelSize modelSize,
		WeightsMetrics metrics,
		IEnumerable<ClassifierTag> tags)
	{
		ClassifierWeights weights = new(modelSize, metrics, tags, this);
		AddWeights(weights);
		return weights;
	}
}