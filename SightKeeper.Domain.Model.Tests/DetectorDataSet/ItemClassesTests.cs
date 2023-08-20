using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests.DetectorDataSet;

public sealed class ItemClassesTests
{
    [Fact]
    public void ShouldNotBeAbleCreateTwoItemClassesWithTheSameName()
    {
        const string itemClassName = "Test item class";
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        dataSet.CreateItemClass(itemClassName);
        Assert.Throws<ArgumentException>(() => dataSet.CreateItemClass(itemClassName));
    }
}