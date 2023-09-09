using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests.DetectorDataSet;

public sealed class WeightsTests
{
    [Fact]
    public void ShouldAddWeights()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var weights = dataSet.WeightsLibrary.CreateWeights(Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Nano, 0, 0, 0, 0, new List<Asset>());
        dataSet.WeightsLibrary.Weights.Should().Contain(weights);
    }
}