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
        var itemClass = model.CreateItemClass("Dummy item class");
        Screenshot screenshot = new(new Image(Array.Empty<byte>()));
        DetectorAsset asset = new(model, screenshot);
        DetectorItem item = new(itemClass, new BoundingBox());
        asset.AddItem(item);
        model.Assets.Add(asset);
        model.Assets.Should().Contain(asset);
    }
}