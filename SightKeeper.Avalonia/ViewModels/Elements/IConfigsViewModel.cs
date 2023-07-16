using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public interface IConfigsViewModel
{
    Task<IReadOnlyCollection<ModelConfig>> Configs { get; }
    ModelConfig? SelectedConfig { get; set; }
    
    IAsyncRelayCommand AddConfigCommand { get; }
    
    IAsyncRelayCommand DeleteConfigCommand { get; }
}