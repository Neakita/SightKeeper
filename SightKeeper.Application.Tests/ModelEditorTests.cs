using FluentValidation;
using SightKeeper.Application.Model.Editing;
using SightKeeper.Data.Services.Model;
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
        DetectorDataSet dataSet = new("Untitled model");
        ModelChangesDTO changes = new(dataSet, "New name", dataSet.Description, dataSet.Resolution, dataSet.ItemClasses, dataSet.Game);
        await editor.ApplyChanges(changes);
        dataSet.Name.Should().Be(changes.Name);
    }

    [Fact]
    public async Task ShouldApplyDescriptionChange()
    {
        var editor = Editor;
        DetectorDataSet dataSet = new("Untitled model");
        ModelChangesDTO changes = new(dataSet, dataSet.Name, "New description", dataSet.Resolution, dataSet.ItemClasses, dataSet.Game);
        await editor.ApplyChanges(changes);
        dataSet.Description.Should().Be(changes.Description);
    }

    [Fact]
    public async Task ShouldApplyResolutionChange()
    {
        var editor = Editor;
        DetectorDataSet dataSet = new("Untitled model");
        Resolution changedResolution = new(640, 640);
        ModelChangesDTO changes = new(dataSet, dataSet.Name, dataSet.Description, changedResolution, dataSet.ItemClasses, dataSet.Game);
        await editor.ApplyChanges(changes);
        dataSet.Resolution.Should().BeEquivalentTo(changedResolution);
    }

    [Fact]
    public async Task ShouldNotApplyResolutionChange()
    {
        var editor = Editor;
        DetectorDataSet dataSet = new("Untitled model");
        Resolution changedResolution = new(636, 636);
        ModelChangesDTO changes = new(dataSet, dataSet.Name, dataSet.Description, changedResolution, dataSet.ItemClasses, dataSet.Game);
        await Assert.ThrowsAsync<ValidationException>(() => editor.ApplyChanges(changes));
        dataSet.Resolution.Should().NotBe(changedResolution);
    }

    [Fact]
    public async Task ShouldAddNewItemClass()
    {
        var editor = Editor;
        DetectorDataSet dataSet = new("Untitled model");
        var newItemClasses = dataSet.ItemClasses.Select(itemClass => itemClass.Name).Append("New item class").ToList();
        ModelChangesDTO changes = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, newItemClasses, dataSet.Game);
        await editor.ApplyChanges(changes);
        dataSet.ItemClasses.Select(itemClass => itemClass.Name).Should().BeEquivalentTo(changes.ItemClasses);
    }

    [Fact]
    public async Task ShouldNotAddDuplicateItemClass()
    {
        const string itemClassName = "Item class";
        var editor = Editor;
        DetectorDataSet dataSet = new("Untitled model");
        dataSet.CreateItemClass(itemClassName);
        var newItemClasses = dataSet.ItemClasses.Select(itemClass => itemClass.Name).Append(itemClassName).ToList();
        ModelChangesDTO changes = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, newItemClasses, dataSet.Game);
        await Assert.ThrowsAsync<ValidationException>(() => editor.ApplyChanges(changes));
        dataSet.ItemClasses.Should().ContainSingle(itemClass => itemClass.Name == itemClassName);
    }

    [Fact]
    public async Task ShouldDeleteItemClass()
    {
        var editor = Editor;
        DetectorDataSet dataSet = new("Untitled model");
        const string itemClassName = "Item class";
        dataSet.CreateItemClass(itemClassName);
        ModelChangesDTO changes = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, new List<ItemClass>(), dataSet.Game);
        await editor.ApplyChanges(changes);
        dataSet.ItemClasses.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldDeleteOnlyOneItemClass()
    {
        var editor = Editor;
        DetectorDataSet dataSet = new("Untitled model");
        const string itemClassName1 = "Item class 1";
        const string itemClassName2 = "Item class 2";
        var itemClass1 = dataSet.CreateItemClass(itemClassName1);
        dataSet.CreateItemClass(itemClassName2);
        ModelChangesDTO changes = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, new List<ItemClass> { itemClass1 }, dataSet.Game);
        await editor.ApplyChanges(changes);
        dataSet.ItemClasses.Should().ContainSingle(itemClass => itemClass == itemClass1);
    }

    [Fact]
    public async Task ShouldNotDeleteItemClassWithAsset()
    {
        var editor = Editor;
        DetectorDataSet dataSet = new("Untitled model");
        var itemClass = dataSet.CreateItemClass("Item class");
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset = dataSet.MakeAsset(screenshot);
        asset.CreateItem(itemClass, new BoundingBox());
        ModelChangesDTO changes = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, new List<ItemClass>(), dataSet.Game);
        await Assert.ThrowsAsync<InvalidOperationException>(() => editor.ApplyChanges(changes));
        dataSet.ItemClasses.Should().Contain(itemClass);
    }

    [Fact]
    public async Task ShouldChangeGame()
    {
        var editor = Editor;
        DetectorDataSet dataSet = new("Untitled model");
        Game newGame = new("New game", "game.exe");
        ModelChangesDTO changes = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, dataSet.ItemClasses, newGame);
        await editor.ApplyChanges(changes);
        dataSet.Game.Should().Be(newGame);
    }

    private DbModelEditor Editor => new(new ModelChangesValidator(), DbContextFactory.CreateDbContext());
}