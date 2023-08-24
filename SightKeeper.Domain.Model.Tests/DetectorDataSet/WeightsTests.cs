using FluentAssertions;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests.DetectorDataSet;

public sealed class WeightsTests
{
    [Fact]
    public void ShouldAddWeights()
    {
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        var weights = dataSet.WeightsLibrary.CreateWeights(Array.Empty<byte>(), ModelSize.Nano, 0, 0);
        dataSet.WeightsLibrary.Weights.Should().Contain(weights);
    }
}