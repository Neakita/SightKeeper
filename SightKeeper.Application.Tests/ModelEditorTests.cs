using SightKeeper.Application.Modelling;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services.Validating;

namespace SightKeeper.Application.Tests;

public sealed class ModelEditorTests
{
    [Fact]
    public void ShouldApplyNameChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelChanges changes = new("New name", model.Description, model.Resolution, model.ItemClasses, model.Game, model.Config);
        editor.ApplyChanges(model, changes);
        model.Name.Should().Be(changes.Name);
    }

    [Fact]
    public void ShouldApplyDescriptionChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelChanges changes = new(model.Name, "New description", model.Resolution, model.ItemClasses, model.Game, model.Config);
        editor.ApplyChanges(model, changes);
        model.Description.Should().Be(changes.Description);
    }

    [Fact]
    public void ShouldApplyResolutionChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelChanges changes = new(model.Name, model.Description, new Resolution(640, 640), model.ItemClasses, model.Game, model.Config);
        editor.ApplyChanges(model, changes);
        model.Resolution.Should().Be(changes.Resolution);
    }

    [Fact]
    public void ShouldNotApplyResolutionChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelChanges changes = new(model.Name, model.Description, new Resolution(636, 636), model.ItemClasses, model.Game, model.Config);
        Assert.Throws<ArgumentException>(() => editor.ApplyChanges(model, changes));
        model.Resolution.Should().NotBe(changes.Resolution);
    }

    [Fact]
    public void ShouldAddNewItemClass()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        var newItemClasses = model.ItemClasses.Append(new ItemClass("New item class")).ToList();
        ModelChanges changes = new(model.Name, model.Description, model.Resolution, newItemClasses, model.Game, model.Config);
        editor.ApplyChanges(model, changes);
        model.ItemClasses.Should().BeEquivalentTo(changes.ItemClasses);
    }

    [Fact]
    public void ShouldNotAddDuplicateItemClass()
    {
        const string itemClassName = "Item class";
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        model.CreateItemClass(itemClassName);
        var newItemClasses = model.ItemClasses.Append(new ItemClass(itemClassName)).ToList();
        ModelChanges changes = new(model.Name, model.Description, model.Resolution, newItemClasses, model.Game, model.Config);
        Assert.Throws<InvalidOperationException>(() => editor.ApplyChanges(model, changes));
        model.ItemClasses.Should().ContainSingle(itemClass => itemClass.Name == itemClassName);
    }

    [Fact]
    public void ShouldDeleteItemClass()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        const string itemClassName = "Item class";
        model.CreateItemClass(itemClassName);
        ModelChanges changes = new(model.Name, model.Description, model.Resolution, new List<ItemClass>(), model.Game, model.Config);
        editor.ApplyChanges(model, changes);
        model.ItemClasses.Should().BeEmpty();
    }

    [Fact]
    public void ShouldDeleteOnlyOneItemClass()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        const string itemClassName1 = "Item class 1";
        const string itemClassName2 = "Item class 2";
        var itemClass1 = model.CreateItemClass(itemClassName1);
        model.CreateItemClass(itemClassName2);
        ModelChanges changes = new(model.Name, model.Description, model.Resolution, new List<ItemClass> { itemClass1 }, model.Game, model.Config);
        editor.ApplyChanges(model, changes);
        model.ItemClasses.Should().ContainSingle(itemClass => itemClass == itemClass1);
    }

    [Fact]
    public void ShouldNotDeleteItemClassWithAsset()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        var itemClass = model.CreateItemClass("Item class");
        Screenshot screenshot = new(new Image(Array.Empty<byte>()));
        model.ScreenshotsLibrary.AddScreenshot(screenshot);
        var asset = model.MakeAssetFromScreenshot(screenshot);
        asset.CreateItem(itemClass, new BoundingBox());
        ModelChanges changes = new(model.Name, model.Description, model.Resolution, new List<ItemClass>(), model.Game, model.Config);
        Assert.Throws<InvalidOperationException>(() => editor.ApplyChanges(model, changes));
        model.ItemClasses.Should().Contain(itemClass);
    }

    [Fact]
    public void ShouldChangeGame()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        Game newGame = new("New game", "game.exe");
        ModelChanges changes = new(model.Name, model.Description, model.Resolution, model.ItemClasses, newGame, model.Config);
        editor.ApplyChanges(model, changes);
        model.Game.Should().Be(newGame);
    }

    [Fact]
    public void ShouldChangeConfig()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelConfig newConfig = new("New game", "game.exe", ModelType.Detector);
        ModelChanges changes = new(model.Name, model.Description, model.Resolution, model.ItemClasses, model.Game, newConfig);
        editor.ApplyChanges(model, changes);
        model.Config.Should().Be(newConfig);
    }

    private static ModelEditorImplementation Editor => new(new ModelValidator(new ResolutionValidator()));
}