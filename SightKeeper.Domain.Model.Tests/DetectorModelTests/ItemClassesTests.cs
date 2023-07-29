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
}