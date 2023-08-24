using FluentValidation;
using NSubstitute;
using SightKeeper.Application.DataSet;
using SightKeeper.Application.DataSet.Editing;
using SightKeeper.Data.Services.DataSet;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Tests.Common;

namespace SightKeeper.Application.Tests;

public sealed class DataSetEditorTests : DbRelatedTests
{
    [Fact]
    public async Task ShouldApplyNameChange()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        DataSetChangesDTO dataSetChanges = new(dataSet, "New name", dataSet.Description, dataSet.Resolution, dataSet.ItemClasses, dataSet.Game);
        await editor.ApplyChanges(dataSetChanges);
        dataSet.Name.Should().Be(dataSetChanges.Name);
    }

    [Fact]
    public async Task ShouldApplyDescriptionChange()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, "New description", dataSet.Resolution, dataSet.ItemClasses, dataSet.Game);
        await editor.ApplyChanges(dataSetChanges);
        dataSet.Description.Should().Be(dataSetChanges.Description);
    }

    [Fact]
    public async Task ShouldApplyResolutionChange()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        Resolution changedResolution = new(640, 640);
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, changedResolution, dataSet.ItemClasses, dataSet.Game);
        await editor.ApplyChanges(dataSetChanges);
        dataSet.Resolution.Should().BeEquivalentTo(changedResolution);
    }

    [Fact]
    public async Task ShouldNotApplyResolutionChange()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        Resolution changedResolution = new(636, 636);
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, changedResolution, dataSet.ItemClasses, dataSet.Game);
        await Assert.ThrowsAsync<ValidationException>(() => editor.ApplyChanges(dataSetChanges));
        dataSet.Resolution.Should().NotBe(changedResolution);
    }

    [Fact]
    public async Task ShouldAddNewItemClass()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        var newItemClasses = dataSet.ItemClasses.Select(itemClass => itemClass.Name).Append("New item class").ToList();
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, newItemClasses, dataSet.Game);
        await editor.ApplyChanges(dataSetChanges);
        dataSet.ItemClasses.Select(itemClass => itemClass.Name).Should().BeEquivalentTo(dataSetChanges.ItemClasses);
    }

    [Fact]
    public async Task ShouldNotAddDuplicateItemClass()
    {
        const string itemClassName = "Item class";
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        dataSet.CreateItemClass(itemClassName);
        var newItemClasses = dataSet.ItemClasses.Select(itemClass => itemClass.Name).Append(itemClassName).ToList();
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, newItemClasses, dataSet.Game);
        await Assert.ThrowsAsync<ValidationException>(() => editor.ApplyChanges(dataSetChanges));
        dataSet.ItemClasses.Should().ContainSingle(itemClass => itemClass.Name == itemClassName);
    }

    [Fact]
    public async Task ShouldDeleteItemClass()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        const string itemClassName = "Item class";
        dataSet.CreateItemClass(itemClassName);
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, new List<ItemClass>(), dataSet.Game);
        await editor.ApplyChanges(dataSetChanges);
        dataSet.ItemClasses.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldDeleteOnlyOneItemClass()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        const string itemClassName1 = "Item class 1";
        const string itemClassName2 = "Item class 2";
        var itemClass1 = dataSet.CreateItemClass(itemClassName1);
        dataSet.CreateItemClass(itemClassName2);
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, new List<ItemClass> { itemClass1 }, dataSet.Game);
        await editor.ApplyChanges(dataSetChanges);
        dataSet.ItemClasses.Should().ContainSingle(itemClass => itemClass == itemClass1);
    }

    [Fact]
    public async Task ShouldNotDeleteItemClassWithAsset()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        var itemClass = dataSet.CreateItemClass("Item class");
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset = dataSet.MakeAsset(screenshot);
        asset.CreateItem(itemClass, new Bounding());
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, new List<ItemClass>(), dataSet.Game);
        await Assert.ThrowsAsync<InvalidOperationException>(() => editor.ApplyChanges(dataSetChanges));
        dataSet.ItemClasses.Should().Contain(itemClass);
    }

    [Fact]
    public async Task ShouldChangeGame()
    {
        var editor = Editor;
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        Game newGame = new("New game", "game.exe");
        DataSetChangesDTO dataSetChanges = new(dataSet, dataSet.Name, dataSet.Description, dataSet.Resolution, dataSet.ItemClasses, newGame);
        await editor.ApplyChanges(dataSetChanges);
        dataSet.Game.Should().Be(newGame);
    }

    private DbDataSetEditor Editor
    {
        get
        {
            var dataSetsDataAccessSubstitute = Substitute.For<DataSetsDataAccess>();
            IValidator<DataSetChanges> validator = new DataSetChangesValidator(new DataSetInfoValidator(), dataSetsDataAccessSubstitute);
            return new DbDataSetEditor(validator, DbContextFactory.CreateDbContext());
        }
    }
}