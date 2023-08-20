using FluentAssertions;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests.DetectorDataSet;

public sealed class WeightsTests
{
    [Fact]
    public void ShouldAddWeights()
    {
        DataSet<DetectorAsset> dataSet = new("Model");
        var weights = dataSet.WeightsLibrary.CreateWeights(Array.Empty<byte>(), ModelSize.Nano, 0, 0);
        dataSet.WeightsLibrary.Weights.Should().Contain(weights);
    }
}