using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Model;

public interface IModel
{
    public string Name { get; }
    public string Description { get; }
    public Resolution Resolution { get; }
    public IReadOnlyCollection<ItemClass> ItemClasses { get; }
    public Game? Game { get; }
    public ModelConfig? Config { get; }
}