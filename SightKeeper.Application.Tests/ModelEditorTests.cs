using SightKeeper.Application.Model;
using SightKeeper.Application.Model.Editing;
using SightKeeper.Data;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Tests;

public sealed class ModelEditorTests
{
    [Fact]
    public void ShouldApplyNameChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelChangesDTO changes = new(model, "New name", model.Description, model.Resolution, model.ItemClasses, model.Game, model.Config);
        editor.ApplyChanges(changes);
        model.Name.Should().Be(changes.Name);
    }

    [Fact]
    public void ShouldApplyDescriptionChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelChangesDTO changes = new(model, model.Name, "New description", model.Resolution, model.ItemClasses, model.Game, model.Config);
        editor.ApplyChanges(changes);
        model.Description.Should().Be(changes.Description);
    }

    [Fact]
    public void ShouldApplyResolutionChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        Resolution changedResolution = new(640, 640);
        ModelChangesDTO changes = new(model, model.Name, model.Description, changedResolution, model.ItemClasses, model.Game, model.Config);
        editor.ApplyChanges(changes);
        model.Resolution.Should().BeEquivalentTo(changedResolution);
    }

    [Fact]
    public void ShouldNotApplyResolutionChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        Resolution changedResolution = new(636, 636);
        ModelChangesDTO changes = new(model, model.Name, model.Description, changedResolution, model.ItemClasses, model.Game, model.Config);
        Assert.Throws<ArgumentException>(() => editor.ApplyChanges(changes));
        model.Resolution.Should().NotBe(changedResolution);
    }

    [Fact]
    public void ShouldAddNewItemClass()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        var newItemClasses = model.ItemClasses.Append(new ItemClass("New item class")).ToList();
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, newItemClasses, model.Game, model.Config);
        editor.ApplyChanges(changes);
        model.ItemClasses.Select(itemClass => itemClass.Name).Should().BeEquivalentTo(changes.ItemClasses);
    }

    [Fact]
    public void ShouldNotAddDuplicateItemClass()
    {
        const string itemClassName = "Item class";
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        model.CreateItemClass(itemClassName);
        var newItemClasses = model.ItemClasses.Append(new ItemClass(itemClassName)).ToList();
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, newItemClasses, model.Game, model.Config);
        Assert.Throws<ArgumentException>(() => editor.ApplyChanges(changes));
        model.ItemClasses.Should().ContainSingle(itemClass => itemClass.Name == itemClassName);
    }

    [Fact]
    public void ShouldDeleteItemClass()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        const string itemClassName = "Item class";
        model.CreateItemClass(itemClassName);
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, new List<ItemClass>(), model.Game, model.Config);
        editor.ApplyChanges(changes);
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
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, new List<ItemClass> { itemClass1 }, model.Game, model.Config);
        editor.ApplyChanges(changes);
        model.ItemClasses.Should().ContainSingle(itemClass => itemClass == itemClass1);
    }

    [Fact]
    public void ShouldNotDeleteItemClassWithAsset()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        var itemClass = model.CreateItemClass("Item class");
        var screenshot = model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        var asset = model.MakeAssetFromScreenshot(screenshot);
        asset.CreateItem(itemClass, new BoundingBox());
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, new List<ItemClass>(), model.Game, model.Config);
        Assert.Throws<InvalidOperationException>(() => editor.ApplyChanges(changes));
        model.ItemClasses.Should().Contain(itemClass);
    }

    [Fact]
    public void ShouldChangeGame()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        Game newGame = new("New game", "game.exe");
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, model.ItemClasses, newGame, model.Config);
        editor.ApplyChanges(changes);
        model.Game.Should().Be(newGame);
    }

    [Fact]
    public void ShouldChangeConfig()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelConfig newConfig = new("New game", "game.exe", ModelType.Detector);
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, model.ItemClasses, model.Game, newConfig);
        editor.ApplyChanges(changes);
        model.Config.Should().Be(newConfig);
    }

    private static DbModelEditor Editor => new(new ModelChangesValidator(), new AppDbContext());
}