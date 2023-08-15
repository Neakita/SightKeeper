using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Config;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Dialogs;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed partial class ConfigsViewModel : ViewModel, IConfigsViewModel
{
    public ReadOnlyCollection<ConfigViewModel> Configs { get; }
    
    public ConfigsViewModel(ILifetimeScope scope, ConfigsDataAccess dataAccess, ConfigCreator configCreator, ConfigEditor configEditor, ConfigsListViewModel configsListViewModel)
    {
        _scope = scope;
        _dataAccess = dataAccess;
        _configCreator = configCreator;
        _configEditor = configEditor;
        Configs = configsListViewModel.Configs;
    }

    private readonly ILifetimeScope _scope;
    private readonly ConfigsDataAccess _dataAccess;
    private readonly ConfigCreator _configCreator;
    private readonly ConfigEditor _configEditor;
    [ObservableProperty] private ConfigViewModel? _selectedConfig;

    [RelayCommand]
    private async Task AddConfig(CancellationToken cancellationToken)
    {
        await using var scope = _scope.BeginLifetimeScope(this);
        var viewModel = scope.Resolve<ConfigEditorViewModel>();
        await viewModel.ShowDialog(this);
        if (viewModel.DialogResult != true) return;
        await _configCreator.CreateConfig(viewModel, cancellationToken);
        OnPropertyChanged(nameof(Configs));
    }

    [RelayCommand]
    private async Task EditConfig(CancellationToken cancellationToken)
    {
        Guard.IsNotNull(SelectedConfig);
        await using var scope = _scope.BeginLifetimeScope(this);
        var viewModel = scope.Resolve<ConfigEditorViewModel>();
        viewModel.SetValues(SelectedConfig.Config);
        await viewModel.ShowDialog(this);
        if (viewModel.DialogResult != true) return;
        await _configEditor.ApplyChanges(new ConfigChangeDTO(SelectedConfig.Config, viewModel), cancellationToken);
        OnPropertyChanged(nameof(Configs));
    }

    [RelayCommand]
    private async Task DeleteConfig(CancellationToken cancellationToken)
    {
        Guard.IsNotNull(SelectedConfig);
        await _dataAccess.RemoveConfig(SelectedConfig.Config, cancellationToken);
        OnPropertyChanged(nameof(Configs));
    }
}