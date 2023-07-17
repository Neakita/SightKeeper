using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Config;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Dialogs;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed partial class ConfigsViewModel : ViewModel, IConfigsViewModel
{
    public Task<IReadOnlyCollection<ModelConfig>> Configs => _dataAccess.GetConfigs();
    
    public ConfigsViewModel(ILifetimeScope scope, ConfigsDataAccess dataAccess, ConfigCreator configCreator, ConfigEditor configEditor)
    {
        _scope = scope;
        _dataAccess = dataAccess;
        _configCreator = configCreator;
        _configEditor = configEditor;
    }

    private readonly ILifetimeScope _scope;
    private readonly ConfigsDataAccess _dataAccess;
    private readonly ConfigCreator _configCreator;
    private readonly ConfigEditor _configEditor;
    [ObservableProperty] private ModelConfig? _selectedConfig;

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
        viewModel.SetValues(SelectedConfig);
        await viewModel.ShowDialog(this);
        if (viewModel.DialogResult != true) return;
        await _configEditor.ApplyChanges(new ConfigChangeDTO(SelectedConfig, viewModel), cancellationToken);
        OnPropertyChanged(nameof(Configs));
    }

    [RelayCommand]
    private async Task DeleteConfig(CancellationToken cancellationToken)
    {
        Guard.IsNotNull(SelectedConfig);
        await _dataAccess.RemoveConfig(SelectedConfig, cancellationToken);
        OnPropertyChanged(nameof(Configs));
    }
}