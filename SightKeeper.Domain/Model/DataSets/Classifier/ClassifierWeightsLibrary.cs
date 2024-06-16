namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierWeightsLibrary : WeightsLibrary<ClassifierWeights>
{
	public ClassifierDataSet DataSet { get; }

	internal ClassifierWeightsLibrary(ClassifierDataSet dataSet)
	{
		DataSet = dataSet;
	}
}