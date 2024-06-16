namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorWeightsLibrary : WeightsLibrary<DetectorWeights>
{
	public DetectorDataSet DataSet { get; }

	internal DetectorWeightsLibrary(DetectorDataSet dataSet)
	{
		DataSet = dataSet;
	}

    internal DetectorWeights CreateWeights(
	    ModelSize modelSize,
	    WeightsMetrics metrics,
	    IEnumerable<DetectorTag> tags)
    {
	    DetectorWeights weights = new(modelSize, metrics, tags, this);
	    AddWeights(weights);
	    return weights;
    }
}