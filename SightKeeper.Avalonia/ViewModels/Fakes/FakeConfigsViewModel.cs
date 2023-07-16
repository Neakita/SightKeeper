using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeConfigsViewModel : IConfigsViewModel
{
    public Task<IReadOnlyCollection<ModelConfig>> Configs { get; }
    public ModelConfig? SelectedConfig { get; set; }
    public IAsyncRelayCommand AddConfigCommand { get; }
    public IAsyncRelayCommand DeleteConfigCommand { get; }
}