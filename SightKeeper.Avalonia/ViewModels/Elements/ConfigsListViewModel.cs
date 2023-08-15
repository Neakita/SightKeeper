using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using Autofac;
using DynamicData;
using SightKeeper.Application.Config;
using SightKeeper.Domain.Model;
using SightKeeper.Services;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class ConfigsListViewModel
{
    public ReadOnlyObservableCollection<ConfigViewModel> Configs { get; }

    public ConfigsListViewModel(ILifetimeScope scope, ConfigsObservableRepository repository, ConfigEditor configEditor)
    {
        repository.Configs.Connect()
            .Transform(config => new ConfigViewModel(config))
            .Bind(out var items)
            .AddKey(viewModel => viewModel.Config)
            .PopulateInto(_cache)
            .DisposeWith(_disposable);
        Configs = items;
        configEditor.ConfigEdited.Subscribe(OnConfigEdited).DisposeWith(_disposable);
    }

    private readonly SourceCache<ConfigViewModel, ModelConfig> _cache = new(viewModel => viewModel.Config);
    private readonly CompositeDisposable _disposable = new();

    private void OnConfigEdited(ModelConfig config) => _cache.Lookup(config).Value.NotifyPropertiesChanged();
}