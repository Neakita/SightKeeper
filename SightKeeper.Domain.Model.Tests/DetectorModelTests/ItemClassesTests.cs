using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests.DetectorModelTests;

public sealed class ItemClassesTests
{
    [Fact]
    public void ShouldNotBeAbleCreateTwoItemClassesWithTheSameName()
    {
        const string itemClassName = "Test item class";
        DetectorDataSet dataSet = new("Dummy model");
        dataSet.CreateItemClass(itemClassName);
        Assert.Throws<ArgumentException>(() => dataSet.CreateItemClass(itemClassName));
    }
}