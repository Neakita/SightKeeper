using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public interface IConfigsViewModel
{
    ReadOnlyCollection<ConfigViewModel> Configs { get; }
    ConfigViewModel? SelectedConfig { get; set; }
    
    IAsyncRelayCommand AddConfigCommand { get; }
    IAsyncRelayCommand EditConfigCommand { get; }
    IAsyncRelayCommand DeleteConfigCommand { get; }
}