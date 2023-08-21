using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.DataSet.Editing;

public sealed class DataSetChangesDTO : DataSetChanges
{
    public Domain.Model.DataSet DataSet { get; }
    public string Name { get; }
    public string Description { get; }
    public int ResolutionWidth { get; }
    int? DataSetInfo.ResolutionWidth => ResolutionWidth;
    public int ResolutionHeight { get; }
    int? DataSetInfo.ResolutionHeight => ResolutionHeight;
    public IReadOnlyCollection<string> ItemClasses { get; }
    public Game? Game { get; }

    public DataSetChangesDTO(Domain.Model.DataSet dataSet, DataSetInfo dataSetInfo)
    {
        Guard.IsNotNull(dataSetInfo.ResolutionWidth);
        Guard.IsNotNull(dataSetInfo.ResolutionHeight);
        DataSet = dataSet;
        Name = dataSetInfo.Name;
        Description = dataSetInfo.Description;
        ResolutionWidth = dataSetInfo.ResolutionWidth.Value;
        ResolutionHeight = dataSetInfo.ResolutionHeight.Value;
        ItemClasses = dataSetInfo.ItemClasses.ToList();
        Game = dataSetInfo.Game;
    }
    
    public DataSetChangesDTO(Domain.Model.DataSet dataSet, string name, string description, Resolution resolution, IEnumerable<ItemClass> itemClasses, Game? game)
    {
        DataSet = dataSet;
        Name = name;
        Description = description;
        ResolutionWidth = resolution.Width;
        ResolutionHeight = resolution.Height;
        ItemClasses = itemClasses.Select(itemClass => itemClass.Name).ToList();
        Game = game;
    }
    
    public DataSetChangesDTO(Domain.Model.DataSet dataSet, string name, string description, Resolution resolution, IEnumerable<string> itemClasses, Game? game)
    {
        DataSet = dataSet;
        Name = name;
        Description = description;
        ResolutionWidth = resolution.Width;
        ResolutionHeight = resolution.Height;
        ItemClasses = itemClasses.ToList();
        Game = game;
    }
    
    public DataSetChangesDTO(Domain.Model.DataSet dataSet, string name, string description, int resolutionWidth, int resolutionHeight, IReadOnlyCollection<string> itemClasses, Game? game)
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