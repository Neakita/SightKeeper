using FluentAssertions;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests;

public sealed class DetectorAssetTests
{
    [Fact]
    public void ShouldJustAddItem()
    {
        DetectorModel model = new("Dummy model");
        ItemClass itemClass = new("Dummy item class");
        model.ItemClasses.Add(itemClass);
        Screenshot screenshot = new(new Image(Array.Empty<byte>()));
        DetectorAsset asset = new(model, screenshot);
        DetectorItem item = new(itemClass, new BoundingBox());
        asset.AddItem(item);
        model.Assets.Add(asset);
        model.Assets.Should().Contain(asset);
    }

    [Fact]
    public void ShouldNotAddItemWhichClassIsNotInModelItemClasses()
    {
        DetectorModel model = new("Dummy model");
        Screenshot screenshot = new(new Image(Array.Empty<byte>()));
        DetectorAsset asset = new(model, screenshot);
        ItemClass itemClass = new("Dummy item class");
        DetectorItem item = new(itemClass, new BoundingBox());
        Assert.Throws<InvalidOperationException>(() => asset.AddItem(item));

    }
}