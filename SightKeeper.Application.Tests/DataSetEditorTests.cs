using FluentValidation;
using NSubstitute;
using SightKeeper.Application.DataSet;
using SightKeeper.Application.DataSet.Editing;
using SightKeeper.Data.Services.DataSet;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet.Screenshots.Assets.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Tests.Common;

namespace SightKeeper.Application.Tests;

public sealed class DataSetEditorTests : DbRelatedTests
{
    [Fact]
    public async Task ShouldApplyNameChange()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        DataSetChangesDTO dataSetChanges = new(dataSet, "New name", dataSet.Description, dataSet.Resolution, dataSet.Game, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), Enumerable.Empty<DeletedItemClass>());
        await editor.ApplyChanges(dataSetChanges);
        dataSet.Name.Should().Be(dataSetChanges.Name);
    }

    [Fact]
    public async Task ShouldApplyDescriptionChange()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, "New description", dataSet.Resolution, dataSet.Game, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), Enumerable.Empty<DeletedItemClass>());
        await editor.ApplyChanges(dataSetChanges);
        dataSet.Description.Should().Be(dataSetChanges.Description);
    }

    [Fact]
    public async Task ShouldApplyResolutionChange()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        const ushort changedResolution = 640;
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, changedResolution, dataSet.Game, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), Enumerable.Empty<DeletedItemClass>());
        await editor.ApplyChanges(dataSetChanges);
        dataSet.Resolution.Should().Be(changedResolution);
    }

    [Fact]
    public async Task ShouldNotApplyResolutionChange()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        const ushort changedResolution = 636;
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, changedResolution, dataSet.Game, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), Enumerable.Empty<DeletedItemClass>());
        await Assert.ThrowsAsync<ValidationException>(() => editor.ApplyChanges(dataSetChanges));
        dataSet.Resolution.Should().NotBe(changedResolution);
    }

    [Fact]
    public async Task ShouldAddNewItemClass()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        const string newItemClassName = "New item class";
        var newItemClasses = new[] { new ItemClassInfo(newItemClassName, 0) };
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, dataSet.Game, newItemClasses, Enumerable.Empty<EditedItemClass>(), Enumerable.Empty<DeletedItemClass>());
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
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, dataSet.Game, newItemClasses, Enumerable.Empty<EditedItemClass>(), Enumerable.Empty<DeletedItemClass>());
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
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, dataSet.Game, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), new []{new DeletedItemClass(itemClass)});
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
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, dataSet.Game, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), new []{new DeletedItemClass(itemClassToDelete)});
        await editor.ApplyChanges(dataSetChanges);
        dataSet.ItemClasses.Should().ContainSingle(itemClass => itemClass == itemClassToKeep);
    }

    [Fact]
    public async Task ShouldNotDeleteItemClassWithAssetAndNoActionProvided()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        var itemClass = dataSet.CreateItemClass("Item class", 0);
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        var asset = dataSet.MakeAsset(screenshot);
        asset.CreateItem(itemClass, new Bounding());
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, dataSet.Game, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), new []{new DeletedItemClass(itemClass)});
        await Assert.ThrowsAsync<ArgumentException>(() => editor.ApplyChanges(dataSetChanges));
        dataSet.ItemClasses.Should().Contain(itemClass);
    }

    [Fact]
    public async Task ShouldChangeGame()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDataSet;
        Game newGame = new("New game", "game.exe");
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, newGame, Enumerable.Empty<ItemClassInfo>(), Enumerable.Empty<EditedItemClass>(), Enumerable.Empty<DeletedItemClass>());
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