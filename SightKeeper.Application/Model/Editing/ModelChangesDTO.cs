using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Model.Editing;

public sealed class ModelChangesDTO : ModelChanges
{
    public Domain.Model.DataSet DataSet { get; }
    public string Name { get; }
    public string Description { get; }
    public int ResolutionWidth { get; }
    int? ModelData.ResolutionWidth => ResolutionWidth;
    public int ResolutionHeight { get; }
    int? ModelData.ResolutionHeight => ResolutionHeight;
    public IReadOnlyCollection<string> ItemClasses { get; }
    public Game? Game { get; }

    public ModelChangesDTO(Domain.Model.DataSet dataSet, ModelData data)
    {
        Guard.IsNotNull(data.ResolutionWidth);
        Guard.IsNotNull(data.ResolutionHeight);
        DataSet = dataSet;
        Name = data.Name;
        Description = data.Description;
        ResolutionWidth = data.ResolutionWidth.Value;
        ResolutionHeight = data.ResolutionHeight.Value;
        ItemClasses = data.ItemClasses.ToList();
        Game = data.Game;
    }
    
    public ModelChangesDTO(Domain.Model.DataSet dataSet, string name, string description, Resolution resolution, IEnumerable<ItemClass> itemClasses, Game? game)
    {
        DataSet = dataSet;
        Name = name;
        Description = description;
        ResolutionWidth = resolution.Width;
        ResolutionHeight = resolution.Height;
        ItemClasses = itemClasses.Select(itemClass => itemClass.Name).ToList();
        Game = game;
    }
    
    public ModelChangesDTO(Domain.Model.DataSet dataSet, string name, string description, Resolution resolution, IEnumerable<string> itemClasses, Game? game)
    {
        DataSet = dataSet;
        Name = name;
        Description = description;
        ResolutionWidth = resolution.Width;
        ResolutionHeight = resolution.Height;
        ItemClasses = itemClasses.ToList();
        Game = game;
    }
    
    public ModelChangesDTO(Domain.Model.DataSet dataSet, string name, string description, int resolutionWidth, int resolutionHeight, IReadOnlyCollection<string> itemClasses, Game? game)
    {
        DataSet = dataSet;
        Name = name;
        Description = description;
        ResolutionWidth = resolutionWidth;
        ResolutionHeight = resolutionHeight;
        ItemClasses = itemClasses;
        Game = game;
    }
}