using FluentAssertions;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests.DetectorDataSet;

public sealed class WeightsTests
{
    [Fact]
    public void ShouldAddWeights()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var weights = dataSet.Weights.CreateWeights(Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Nano, new WeightsMetrics(), dataSet.ItemClasses);
        dataSet.Weights.Records.Should().Contain(weights);
    }
}