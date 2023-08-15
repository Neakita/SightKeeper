using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class ConfigViewModel : ViewModel
{
    public ModelConfig Config { get; }

    public string Name => Config.Name;
    public string Content => Config.Content;
    public ModelType ModelType => Config.ModelType;
    

    public ConfigViewModel(ModelConfig config)
    {
        Config = config;
    }

    public void NotifyPropertiesChanged()
    {
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Content));
        OnPropertyChanged(nameof(ModelType));
    }
}