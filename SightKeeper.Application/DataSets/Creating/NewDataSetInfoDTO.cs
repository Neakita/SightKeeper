using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.DataSets.Creating;

public sealed class NewDataSetInfoDTO : NewDataSetInfo
{
    public string Name { get; }
    public string Description { get; }
    public ushort Resolution { get; }
    public IReadOnlyCollection<ItemClassInfo> ItemClasses { get; }
    public Game? Game { get; }
    int? NewDataSetInfo.Resolution => Resolution;

    public NewDataSetInfoDTO(NewDataSetInfo dataSetInfo)
    {
        Guard.IsNotNull(dataSetInfo.Resolution);
        Name = dataSetInfo.Name;
        Description = dataSetInfo.Description;
        Resolution = (ushort)dataSetInfo.Resolution.Value;
        ItemClasses = dataSetInfo.ItemClasses.ToList();
        Game = dataSetInfo.Game;
    }
    
    public NewDataSetInfoDTO(
        string name,
        string description,
        ushort resolution,
        IReadOnlyCollection<ItemClassInfo> itemClasses,
        Game? game)
    {
        Name = name;
        Description = description;
        Resolution = resolution;
        ItemClasses = itemClasses;
        Game = game;
    }
}