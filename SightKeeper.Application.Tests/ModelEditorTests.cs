using FluentValidation;
using SightKeeper.Application.Model.Editing;
using SightKeeper.Data.Services.Model;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Application.Tests;

public sealed class ModelEditorTests : DbRelatedTests
{
    [Fact]
    public async Task ShouldApplyNameChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelChangesDTO changes = new(model, "New name", model.Description, model.Resolution, model.ItemClasses, model.Game, model.Config);
        await editor.ApplyChanges(changes);
        model.Name.Should().Be(changes.Name);
    }

    [Fact]
    public async Task ShouldApplyDescriptionChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelChangesDTO changes = new(model, model.Name, "New description", model.Resolution, model.ItemClasses, model.Game, model.Config);
        await editor.ApplyChanges(changes);
        model.Description.Should().Be(changes.Description);
    }

    [Fact]
    public async Task ShouldApplyResolutionChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        Resolution changedResolution = new(640, 640);
        ModelChangesDTO changes = new(model, model.Name, model.Description, changedResolution, model.ItemClasses, model.Game, model.Config);
        await editor.ApplyChanges(changes);
        model.Resolution.Should().BeEquivalentTo(changedResolution);
    }

    [Fact]
    public async Task ShouldNotApplyResolutionChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        Resolution changedResolution = new(636, 636);
        ModelChangesDTO changes = new(model, model.Name, model.Description, changedResolution, model.ItemClasses, model.Game, model.Config);
        await Assert.ThrowsAsync<ValidationException>(() => editor.ApplyChanges(changes));
        model.Resolution.Should().NotBe(changedResolution);
    }

    [Fact]
    public async Task ShouldAddNewItemClass()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        var newItemClasses = model.ItemClasses.Select(itemClass => itemClass.Name).Append("New item class").ToList();
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, newItemClasses, model.Game, model.Config);
        await editor.ApplyChanges(changes);
        model.ItemClasses.Select(itemClass => itemClass.Name).Should().BeEquivalentTo(changes.ItemClasses);
    }

    [Fact]
    public async Task ShouldNotAddDuplicateItemClass()
    {
        const string itemClassName = "Item class";
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        model.CreateItemClass(itemClassName);
        var newItemClasses = model.ItemClasses.Select(itemClass => itemClass.Name).Append(itemClassName).ToList();
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, newItemClasses, model.Game, model.Config);
        await Assert.ThrowsAsync<ValidationException>(() => editor.ApplyChanges(changes));
        model.ItemClasses.Should().ContainSingle(itemClass => itemClass.Name == itemClassName);
    }

    [Fact]
    public async Task ShouldDeleteItemClass()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        const string itemClassName = "Item class";
        model.CreateItemClass(itemClassName);
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, new List<ItemClass>(), model.Game, model.Config);
        await editor.ApplyChanges(changes);
        model.ItemClasses.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldDeleteOnlyOneItemClass()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        const string itemClassName1 = "Item class 1";
        const string itemClassName2 = "Item class 2";
        var itemClass1 = model.CreateItemClass(itemClassName1);
        model.CreateItemClass(itemClassName2);
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, new List<ItemClass> { itemClass1 }, model.Game, model.Config);
        await editor.ApplyChanges(changes);
        model.ItemClasses.Should().ContainSingle(itemClass => itemClass == itemClass1);
    }

    [Fact]
    public async Task ShouldNotDeleteItemClassWithAsset()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        var itemClass = model.CreateItemClass("Item class");
        var screenshot = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        var asset = model.MakeAsset(screenshot);
        asset.CreateItem(itemClass, new BoundingBox());
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, new List<ItemClass>(), model.Game, model.Config);
        await Assert.ThrowsAsync<InvalidOperationException>(() => editor.ApplyChanges(changes));
        model.ItemClasses.Should().Contain(itemClass);
    }

    [Fact]
    public async Task ShouldChangeGame()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        Game newGame = new("New game", "game.exe");
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, model.ItemClasses, newGame, model.Config);
        await editor.ApplyChanges(changes);
        model.Game.Should().Be(newGame);
    }

    [Fact]
    public async Task ShouldChangeConfig()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelConfig newConfig = new("New game", "game.exe", ModelType.Detector);
        ModelChangesDTO changes = new(model, model.Name, model.Description, model.Resolution, model.ItemClasses, model.Game, newConfig);
        await editor.ApplyChanges(changes);
        model.Config.Should().Be(newConfig);
    }

    private DbModelEditor Editor => new(new ModelChangesValidator(), DbContextFactory.CreateDbContext());
}