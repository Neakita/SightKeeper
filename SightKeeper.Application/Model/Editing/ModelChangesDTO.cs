using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Model.Editing;

public sealed class ModelChangesDTO : ModelChanges
{
    public Domain.Model.Model Model { get; }
    public string Name { get; }
    public string Description { get; }
    public int ResolutionWidth { get; }
    int? ModelData.ResolutionWidth => ResolutionWidth;
    public int ResolutionHeight { get; }
    int? ModelData.ResolutionHeight => ResolutionHeight;
    public IReadOnlyCollection<string> ItemClasses { get; }
    public Game? Game { get; }
    public ModelConfig? Config { get; }

    public ModelChangesDTO(Domain.Model.Model model, ModelData data)
    {
        Guard.IsNotNull(data.ResolutionWidth);
        Guard.IsNotNull(data.ResolutionHeight);
        Model = model;
        Name = data.Name;
        Description = data.Description;
        ResolutionWidth = data.ResolutionWidth.Value;
        ResolutionHeight = data.ResolutionHeight.Value;
        ItemClasses = data.ItemClasses.ToList();
        Game = data.Game;
        Config = data.Config;
    }
    
    public ModelChangesDTO(Domain.Model.Model model, string name, string description, Resolution resolution, IEnumerable<ItemClass> itemClasses, Game? game, ModelConfig? config)
    {
        Model = model;
        Name = name;
        Description = description;
        ResolutionWidth = resolution.Width;
        ResolutionHeight = resolution.Height;
        ItemClasses = itemClasses.Select(itemClass => itemClass.Name).ToList();
        Game = game;
        Config = config;
    }
    
    public ModelChangesDTO(Domain.Model.Model model, string name, string description, Resolution resolution, IEnumerable<string> itemClasses, Game? game, ModelConfig? config)
    {
        Model = model;
        Name = name;
        Description = description;
        ResolutionWidth = resolution.Width;
        ResolutionHeight = resolution.Height;
        ItemClasses = itemClasses.ToList();
        Game = game;
        Config = config;
    }
    
    public ModelChangesDTO(Domain.Model.Model model, string name, string description, int resolutionWidth, int resolutionHeight, IReadOnlyCollection<string> itemClasses, Game? game, ModelConfig? config)
    {
        Model = model;
        Name = name;
        Description = description;
        ResolutionWidth = resolutionWidth;
        ResolutionHeight = resolutionHeight;
        ItemClasses = itemClasses;
        Game = game;
        Config = config;
    }
}