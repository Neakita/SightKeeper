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

    [Fact]
    public void ShouldNotAddWeightsWithNotBelongingItemClasses()
    {
	    SimpleWeightsDataAccess weightsDataAccess = new();
	    var dataSet1 = DomainTestsHelper.NewDataSet;
	    var dataSet2 = DomainTestsHelper.NewDataSet;
	    dataSet2.CreateItemClass("Test item class", 0);
	    Assert.Throws<ArgumentException>(() => weightsDataAccess.CreateWeights(dataSet1.Weights, Array.Empty<byte>(),
		    Array.Empty<byte>(), ModelSize.Nano, new WeightsMetrics(), dataSet2.ItemClasses));
	    dataSet1.Weights.Should().BeEmpty();
    }

    [Fact]
    public void ShouldRemoveWeights()
    {
	    SimpleWeightsDataAccess weightsDataAccess = new();
	    var dataSet = DomainTestsHelper.NewDataSet;
	    var weights = weightsDataAccess.CreateWeights(dataSet.Weights, Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Nano, new WeightsMetrics(), dataSet.ItemClasses);
	    weightsDataAccess.RemoveWeights(weights);
	    weights.Library.Should().NotContain(weights);
    }
}