using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class ModelViewModel : ViewModel
{
    private static readonly string[] Properties =
    {
        nameof(Name),
        nameof(Description),
        nameof(Game),
        nameof(Config),
        nameof(Resolution)
    };
    
    public Model Model { get; }

    public string Name => Model.Name;
    public string Description => Model.Description;
    public Game? Game => Model.Game;
    public ModelConfig? Config => Model.Config;
    public Resolution Resolution => Model.Resolution;

    public ModelViewModel(Model model)
    {
        Model = model;
    }

    public void NotifyChanges() => OnPropertiesChanged(Properties);

    public override string ToString() => Name;
}