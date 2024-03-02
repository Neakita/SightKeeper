using FluentAssertions;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests.DetectorDataSet;

public sealed class WeightsTests
{
    [Fact]
    public void ShouldAddWeights()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var weights = dataSet.WeightsLibrary.CreateWeights(Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Nano, new WeightsMetrics(), dataSet.ItemClasses);
        dataSet.WeightsLibrary.Weights.Should().Contain(weights);
    }
}