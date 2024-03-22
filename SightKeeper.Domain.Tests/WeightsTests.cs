using FluentAssertions;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Tests;

public sealed class WeightsTests
{
    [Fact]
    public void ShouldAddWeights()
    {
	    SimpleWeightsDataAccess weightsDataAccess = new();
        var dataSet = DomainTestsHelper.NewDataSet;
        var weights = weightsDataAccess.CreateWeights(dataSet.Weights, Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Nano, new WeightsMetrics(), dataSet.ItemClasses);
        dataSet.Weights.Should().Contain(weights);
    }
}