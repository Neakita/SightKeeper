using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests.DetectorModelTests;

public sealed class ItemClassesTests
{
    [Fact]
    public void ShouldNotBeAbleCreateTwoItemClassesWithTheSameName()
    {
        const string itemClassName = "Test item class";
        DetectorModel model = new("Dummy model");
        model.CreateItemClass(itemClassName);
        Assert.Throws<ArgumentException>(() => model.CreateItemClass(itemClassName));
    }
    
    [Fact]
    public void ShouldNotAddDuplicateItemClasses()
    {
        DetectorModel model = new("Model");
        ItemClass itemClass = new("Item class");
        model.AddItemClass(itemClass);
        Assert.Throws<ArgumentException>(() => model.AddItemClass(itemClass));
    }
}