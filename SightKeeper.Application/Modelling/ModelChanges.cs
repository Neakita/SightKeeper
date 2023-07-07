using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Model;

namespace SightKeeper.Application.Modelling;

public sealed class ModelChanges : IModel
{
    public string Name { get; }
    public string Description { get; }
    public Resolution Resolution { get; }
    public IReadOnlyCollection<ItemClass> ItemClasses { get; }
    public Game? Game { get; }
    public ModelConfig? Config { get; }
    
    public ModelChanges(
        string name,
        string description,
        Resolution resolution,
        IReadOnlyCollection<ItemClass> itemClasses,
        Game? game,
        ModelConfig? config)
    {
        Name = name;
        Description = description;
        Resolution = resolution;
        ItemClasses = itemClasses;
        Game = game;
        Config = config;
    }
}