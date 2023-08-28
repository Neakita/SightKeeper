using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.DataSet.Editing;

public sealed class DataSetChangesDTO : DataSetChanges
{
    public Domain.Model.DataSet DataSet { get; }
    public string Name { get; }
    public string Description { get; }
    public ushort Resolution { get; }
    int? DataSetInfo.Resolution => Resolution;
    public IReadOnlyCollection<string> ItemClasses { get; }
    public Game? Game { get; }

    public DataSetChangesDTO(Domain.Model.DataSet dataSet, DataSetInfo dataSetInfo)
    {
        Guard.IsNotNull(dataSetInfo.Resolution);
        DataSet = dataSet;
        Name = dataSetInfo.Name;
        Description = dataSetInfo.Description;
        Resolution = (ushort)dataSetInfo.Resolution.Value;
        ItemClasses = dataSetInfo.ItemClasses.ToList();
        Game = dataSetInfo.Game;
    }
    
    public DataSetChangesDTO(Domain.Model.DataSet dataSet, string name, string description, ushort resolution, IEnumerable<ItemClass> itemClasses, Game? game)
    {
        DataSet = dataSet;
        Name = name;
        Description = description;
        Resolution = resolution;
        ItemClasses = itemClasses.Select(itemClass => itemClass.Name).ToList();
        Game = game;
    }
    
    public DataSetChangesDTO(Domain.Model.DataSet dataSet, string name, string description, ushort resolution, IEnumerable<string> itemClasses, Game? game)
    {
        DataSet = dataSet;
        Name = name;
        Description = description;
        Resolution = resolution;
        ItemClasses = itemClasses.ToList();
        Game = game;
    }
    
    public DataSetChangesDTO(Domain.Model.DataSet dataSet, string name, string description, ushort resolution, IReadOnlyCollection<string> itemClasses, Game? game)
    {
        DataSet = dataSet;
        Name = name;
        Description = description;
        Resolution = resolution;
        ItemClasses = itemClasses;
        Game = game;
    }
}