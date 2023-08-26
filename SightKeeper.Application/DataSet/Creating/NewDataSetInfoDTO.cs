using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.DataSet.Creating;

public sealed class NewDataSetInfoDTO : NewDataSetInfo
{
    public string Name { get; }
    public string Description { get; }
    public int ResolutionWidth { get; }
    public int ResolutionHeight { get; }
    public IReadOnlyCollection<string> ItemClasses { get; }
    public Game? Game { get; }
    int? DataSetInfo.ResolutionWidth => ResolutionWidth;
    int? DataSetInfo.ResolutionHeight => ResolutionHeight;
    public Resolution Resolution => new((ushort)ResolutionWidth, (ushort)ResolutionHeight);

    public NewDataSetInfoDTO(DataSetInfo dataSetInfo)
    {
        Guard.IsNotNull(dataSetInfo.ResolutionWidth);
        Guard.IsNotNull(dataSetInfo.ResolutionHeight);
        Name = dataSetInfo.Name;
        Description = dataSetInfo.Description;
        ResolutionWidth = dataSetInfo.ResolutionWidth.Value;
        ResolutionHeight = dataSetInfo.ResolutionHeight.Value;
        ItemClasses = dataSetInfo.ItemClasses.ToList();
        Game = dataSetInfo.Game;
    }
    
    public NewDataSetInfoDTO(
        string name,
        string description,
        int resolutionWidth,
        int resolutionHeight,
        IReadOnlyCollection<string> itemClasses,
        Game? game)
    {
        Name = name;
        Description = description;
        ResolutionWidth = resolutionWidth;
        ResolutionHeight = resolutionHeight;
        ItemClasses = itemClasses;
        Game = game;
    }
}