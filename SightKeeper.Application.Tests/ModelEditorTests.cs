using SightKeeper.Application.Modelling;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Tests;

public sealed class ModelEditorTests
{
    private sealed class ModelChangesMock : ModelChanges
    {
        public Model Model { get; }
        public string Name { get; }
        public string Description { get; }
        public int ResolutionWidth { get; }
        public int ResolutionHeight { get; }
        public IReadOnlyCollection<string> ItemClasses { get; }
        public Game? Game { get; }
        public ModelConfig? Config { get; }
        
        public ModelChangesMock(Model model, string name, string description, Resolution resolution, IEnumerable<ItemClass> itemClasses, Game? game, ModelConfig? config)
        {
            Model = model;
            Name = name;
            Description = description;
            ResolutionWidth = resolution.Width;
            ResolutionHeight = resolution.Width;
            ItemClasses = itemClasses.Select(itemClass => itemClass.Name).ToList();
            Game = game;
            Config = config;
        }
    }
    
    [Fact]
    public void ShouldApplyNameChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelChangesMock changes = new(model, "New name", model.Description, model.Resolution, model.ItemClasses, model.Game, model.Config);
        editor.ApplyChanges(model, changes);
        model.Name.Should().Be(changes.Name);
    }

    [Fact]
    public void ShouldApplyDescriptionChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelChangesMock changes = new(model, model.Name, "New description", model.Resolution, model.ItemClasses, model.Game, model.Config);
        editor.ApplyChanges(model, changes);
        model.Description.Should().Be(changes.Description);
    }

    [Fact]
    public void ShouldApplyResolutionChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        Resolution changedResolution = new(640, 640);
        ModelChangesMock changes = new(model, model.Name, model.Description, changedResolution, model.ItemClasses, model.Game, model.Config);
        editor.ApplyChanges(model, changes);
        model.Resolution.Should().BeEquivalentTo(changedResolution);
    }

    [Fact]
    public void ShouldNotApplyResolutionChange()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        Resolution changedResolution = new(636, 636);
        ModelChangesMock changes = new(model, model.Name, model.Description, changedResolution, model.ItemClasses, model.Game, model.Config);
        Assert.Throws<ArgumentException>(() => editor.ApplyChanges(model, changes));
        model.Resolution.Should().NotBe(changedResolution);
    }

    [Fact]
    public void ShouldAddNewItemClass()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        var newItemClasses = model.ItemClasses.Append(new ItemClass("New item class")).ToList();
        ModelChangesMock changes = new(model, model.Name, model.Description, model.Resolution, newItemClasses, model.Game, model.Config);
        editor.ApplyChanges(model, changes);
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
        ModelChangesMock changes = new(model, model.Name, model.Description, model.Resolution, newItemClasses, model.Game, model.Config);
        Assert.Throws<ArgumentException>(() => editor.ApplyChanges(model, changes));
        model.ItemClasses.Should().ContainSingle(itemClass => itemClass.Name == itemClassName);
    }

    [Fact]
    public void ShouldDeleteItemClass()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        const string itemClassName = "Item class";
        model.CreateItemClass(itemClassName);
        ModelChangesMock changes = new(model, model.Name, model.Description, model.Resolution, new List<ItemClass>(), model.Game, model.Config);
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
        ModelChangesMock changes = new(model, model.Name, model.Description, model.Resolution, new List<ItemClass> { itemClass1 }, model.Game, model.Config);
        editor.ApplyChanges(model, changes);
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
        ModelChangesMock changes = new(model, model.Name, model.Description, model.Resolution, new List<ItemClass>(), model.Game, model.Config);
        Assert.Throws<InvalidOperationException>(() => editor.ApplyChanges(model, changes));
        model.ItemClasses.Should().Contain(itemClass);
    }

    [Fact]
    public void ShouldChangeGame()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        Game newGame = new("New game", "game.exe");
        ModelChangesMock changes = new(model, model.Name, model.Description, model.Resolution, model.ItemClasses, newGame, model.Config);
        editor.ApplyChanges(model, changes);
        model.Game.Should().Be(newGame);
    }

    [Fact]
    public void ShouldChangeConfig()
    {
        var editor = Editor;
        DetectorModel model = new("Untitled model");
        ModelConfig newConfig = new("New game", "game.exe", ModelType.Detector);
        ModelChangesMock changes = new(model, model.Name, model.Description, model.Resolution, model.ItemClasses, model.Game, newConfig);
        editor.ApplyChanges(model, changes);
        model.Config.Should().Be(newConfig);
    }

    private static ModelEditorImplementation Editor => new(new ModelChangesValidator());
}