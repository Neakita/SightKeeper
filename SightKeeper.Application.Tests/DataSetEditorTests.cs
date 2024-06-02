using FluentValidation;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Data.Services.DataSets;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Tests;
using SightKeeper.Tests.Common;

namespace SightKeeper.Application.Tests;

public sealed class DataSetEditorTests : DbRelatedTests
{
    [Fact]
    public async Task ShouldApplyNameChange()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        DataSetChangesDTO dataSetChanges = new(dataSet, "New name", dataSet.Description, dataSet.Game, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), Enumerable.Empty<DeletedItemClass>());
        await editor.ApplyChanges(dataSetChanges);
        dataSet.Name.Should().Be(dataSetChanges.Name);
    }

    [Fact]
    public async Task ShouldApplyDescriptionChange()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, "New description", dataSet.Game, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), Enumerable.Empty<DeletedItemClass>());
        await editor.ApplyChanges(dataSetChanges);
        dataSet.Description.Should().Be(dataSetChanges.Description);
    }

    [Fact]
    public async Task ShouldAddNewItemClass()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        const string newItemClassName = "New item class";
        var newItemClasses = new[] { new ItemClassInfo(newItemClassName, 0) };
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Game, newItemClasses, Enumerable.Empty<EditedItemClass>(), Enumerable.Empty<DeletedItemClass>());
        await editor.ApplyChanges(dataSetChanges);
        dataSet.ItemClasses.Should().ContainSingle(itemClass => itemClass.Name == newItemClassName && itemClass.Color == 0);
    }

    [Fact]
    public async Task ShouldNotAddDuplicateItemClass()
    {
        const string itemClassName = "Item class";
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        dataSet.CreateItemClass(itemClassName, 0);
        var newItemClasses = new[] { new ItemClassInfo(itemClassName, 0) };
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Game, newItemClasses, Enumerable.Empty<EditedItemClass>(), Enumerable.Empty<DeletedItemClass>());
        await Assert.ThrowsAsync<ValidationException>(() => editor.ApplyChanges(dataSetChanges));
        dataSet.ItemClasses.Should().ContainSingle(itemClass => itemClass.Name == itemClassName);
    }

    [Fact]
    public async Task ShouldDeleteItemClass()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        const string itemClassName = "Item class";
        var itemClass = dataSet.CreateItemClass(itemClassName, 0);
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Game, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), new []{new DeletedItemClass(itemClass)});
        await editor.ApplyChanges(dataSetChanges);
        dataSet.ItemClasses.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldDeleteOnlyOneItemClass()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        const string itemClassName1 = "Item class 1";
        const string itemClassName2 = "Item class 2";
        var itemClassToKeep = dataSet.CreateItemClass(itemClassName1, 0);
        var itemClassToDelete = dataSet.CreateItemClass(itemClassName2, 0);
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Game, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), new []{new DeletedItemClass(itemClassToDelete)});
        await editor.ApplyChanges(dataSetChanges);
        dataSet.ItemClasses.Should().ContainSingle(itemClass => itemClass == itemClassToKeep);
    }

    [Fact]
    public async Task ShouldNotDeleteItemClassWithAssetAndNoActionProvided()
    {
        var editor = Editor;
        SimpleScreenshotsDataAccess screenshotsDataAccess = new();
        var dataSet = DomainTestsHelper.NewDataSet;
        var itemClass = dataSet.CreateItemClass("Item class", 0);
        var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        var asset = dataSet.Assets.MakeAsset(screenshot);
        asset.CreateItem(itemClass, new Bounding());
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Game, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), new []{new DeletedItemClass(itemClass)});
        await Assert.ThrowsAsync<ArgumentException>(() => editor.ApplyChanges(dataSetChanges));
        dataSet.ItemClasses.Should().Contain(itemClass);
        throw new NotImplementedException();
    }

    [Fact]
    public async Task ShouldChangeGame()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        Game newGame = new("New game", "game.exe");
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, newGame, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), Enumerable.Empty<DeletedItemClass>());
        await editor.ApplyChanges(dataSetChanges);
        dataSet.Game.Should().Be(newGame);
    }

    private DbDataSetEditor Editor
    {
        get
        {
	        var dbContext = DbContextFactory.CreateDbContext();
	        DbDataSetsDataAccess dataSetsDataAccessSubstitute = new(dbContext);
            IValidator<DataSetChanges> validator = new DataSetChangesValidator(new DataSetInfoValidator(), dataSetsDataAccessSubstitute);
            return new DbDataSetEditor(validator, dbContext);
        }
    }
}