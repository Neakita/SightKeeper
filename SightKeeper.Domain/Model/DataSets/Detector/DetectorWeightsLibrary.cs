namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorWeightsLibrary : WeightsLibrary<DetectorWeights>
{
	public override DetectorDataSet DataSet { get; }

	internal DetectorWeightsLibrary(DetectorDataSet dataSet)
	{
		DataSet = dataSet;
	}

    internal DetectorWeights CreateWeights(
	    ModelSize size,
	    WeightsMetrics metrics,
	    IEnumerable<DetectorTag> tags)
    {
	    DetectorWeights weights = new(size, metrics, tags, this);
	    AddWeights(weights);
	    return weights;
    }
}