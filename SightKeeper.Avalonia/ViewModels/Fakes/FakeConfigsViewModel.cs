using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeConfigsViewModel : IConfigsViewModel
{
    public ReadOnlyCollection<ConfigViewModel> Configs { get; } = new(new List<ConfigViewModel>
    {
        new(new ModelConfig("Test config 1", string.Empty, ModelType.Detector)),
        new(new ModelConfig("Test config 2", string.Empty, ModelType.Classifier))
    });

    public ConfigViewModel? SelectedConfig { get; set; }
    public IAsyncRelayCommand AddConfigCommand { get; } = new AsyncRelayCommand(() => Task.CompletedTask);
    public IAsyncRelayCommand EditConfigCommand { get; } = new AsyncRelayCommand(() => Task.CompletedTask);
    public IAsyncRelayCommand DeleteConfigCommand { get; } = new AsyncRelayCommand(() => Task.CompletedTask);
}