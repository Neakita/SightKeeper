using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests;

public sealed class ItemClassesTests
{
    [Fact]
    public void ShouldNotBeAbleCreateTwoItemClassesWithTheSameName()
    {
        const string itemClassName = "Test item class";
        var dataSet = DomainTestsHelper.NewDataSet;
        dataSet.CreateItemClass(itemClassName, 0);
        Assert.Throws<ArgumentException>(() => dataSet.CreateItemClass(itemClassName, 0));
    }
}