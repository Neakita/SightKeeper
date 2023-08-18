using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Model.Editing;

public sealed class DataSetDataSetDataChangesDTO : DataSetDataChanges
{
    public Domain.Model.DataSet DataSet { get; }
    public string Name { get; }
    public string Description { get; }
    public int ResolutionWidth { get; }
    int? DataSetData.ResolutionWidth => ResolutionWidth;
    public int ResolutionHeight { get; }
    int? DataSetData.ResolutionHeight => ResolutionHeight;
    public IReadOnlyCollection<string> ItemClasses { get; }
    public Game? Game { get; }

    public DataSetDataSetDataChangesDTO(Domain.Model.DataSet dataSet, DataSetData dataSetData)
    {
        Guard.IsNotNull(dataSetData.ResolutionWidth);
        Guard.IsNotNull(dataSetData.ResolutionHeight);
        DataSet = dataSet;
        Name = dataSetData.Name;
        Description = dataSetData.Description;
        ResolutionWidth = dataSetData.ResolutionWidth.Value;
        ResolutionHeight = dataSetData.ResolutionHeight.Value;
        ItemClasses = dataSetData.ItemClasses.ToList();
        Game = dataSetData.Game;
    }
    
    public DataSetDataSetDataChangesDTO(Domain.Model.DataSet dataSet, string name, string description, Resolution resolution, IEnumerable<ItemClass> itemClasses, Game? game)
    {
        DataSet = dataSet;
        Name = name;
        Description = description;
        ResolutionWidth = resolution.Width;
        ResolutionHeight = resolution.Height;
        ItemClasses = itemClasses.Select(itemClass => itemClass.Name).ToList();
        Game = game;
    }
    
    public DataSetDataSetDataChangesDTO(Domain.Model.DataSet dataSet, string name, string description, Resolution resolution, IEnumerable<string> itemClasses, Game? game)
    {
        DataSet = dataSet;
        Name = name;
        Description = description;
        ResolutionWidth = resolution.Width;
        ResolutionHeight = resolution.Height;
        ItemClasses = itemClasses.ToList();
        Game = game;
    }
    
    public DataSetDataSetDataChangesDTO(Domain.Model.DataSet dataSet, string name, string description, int resolutionWidth, int resolutionHeight, IReadOnlyCollection<string> itemClasses, Game? game)
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