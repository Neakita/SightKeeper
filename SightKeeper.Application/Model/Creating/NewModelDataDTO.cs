using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Model.Creating;

public sealed class NewModelDataDTO : NewModelData
{
    public ModelType ModelType { get; }
    public string Name { get; }
    public string Description { get; }
    public int ResolutionWidth { get; }
    public int ResolutionHeight { get; }
    public IReadOnlyCollection<string> ItemClasses { get; }
    public Game? Game { get; }
    public ModelConfig? Config { get; }
    int? ModelData.ResolutionWidth => ResolutionWidth;
    int? ModelData.ResolutionHeight => ResolutionHeight;
    public Resolution Resolution => new(ResolutionWidth, ResolutionHeight);

    public NewModelDataDTO(ModelType type, ModelData data)
    {
        Guard.IsNotNull(data.ResolutionWidth);
        Guard.IsNotNull(data.ResolutionHeight);
        ModelType = type;
        Name = data.Name;
        Description = data.Description;
        ResolutionWidth = data.ResolutionWidth.Value;
        ResolutionHeight = data.ResolutionHeight.Value;
        ItemClasses = data.ItemClasses.ToList();
        Game = data.Game;
        Config = data.Config;
    }
    
    public NewModelDataDTO(ModelType modelType, string name, string description, int resolutionWidth, int resolutionHeight, IReadOnlyCollection<string> itemClasses, Game? game, ModelConfig? config)
    {
        ModelType = modelType;
        Name = name;
        Description = description;
        ResolutionWidth = resolutionWidth;
        ResolutionHeight = resolutionHeight;
        ItemClasses = itemClasses;
        Game = game;
        Config = config;
    }
}