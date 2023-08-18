using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Model.Creating;

public sealed class NewDataSetDataSetDataSetDataDTO : NewDataSetData
{
    public ModelType ModelType { get; }
    public string Name { get; }
    public string Description { get; }
    public int ResolutionWidth { get; }
    public int ResolutionHeight { get; }
    public IReadOnlyCollection<string> ItemClasses { get; }
    public Game? Game { get; }
    int? DataSetData.ResolutionWidth => ResolutionWidth;
    int? DataSetData.ResolutionHeight => ResolutionHeight;
    public Resolution Resolution => new(ResolutionWidth, ResolutionHeight);

    public NewDataSetDataSetDataSetDataDTO(ModelType type, DataSetData dataSetData)
    {
        Guard.IsNotNull(dataSetData.ResolutionWidth);
        Guard.IsNotNull(dataSetData.ResolutionHeight);
        ModelType = type;
        Name = dataSetData.Name;
        Description = dataSetData.Description;
        ResolutionWidth = dataSetData.ResolutionWidth.Value;
        ResolutionHeight = dataSetData.ResolutionHeight.Value;
        ItemClasses = dataSetData.ItemClasses.ToList();
        Game = dataSetData.Game;
    }
    
    public NewDataSetDataSetDataSetDataDTO(
        ModelType modelType,
        string name,
        string description,
        int resolutionWidth,
        int resolutionHeight,
        IReadOnlyCollection<string> itemClasses,
        Game? game)
    {
        ModelType = modelType;
        Name = name;
        Description = description;
        ResolutionWidth = resolutionWidth;
        ResolutionHeight = resolutionHeight;
        ItemClasses = itemClasses;
        Game = game;
    }
}