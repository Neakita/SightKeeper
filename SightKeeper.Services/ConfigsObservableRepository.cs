using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Commons;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services;

public sealed class ConfigsObservableRepository : IDisposable
{
    public IObservableList<ModelConfig> Configs => _source;

    public ConfigsObservableRepository(ConfigsDataAccess configsDataAccess)
    {
        AddInitialConfigs(configsDataAccess);
        _source.DisposeWithEx(_disposable);
        configsDataAccess.ConfigAdded.Subscribe(OnModelAdded).DisposeWithEx(_disposable);
        configsDataAccess.ConfigRemoved.Subscribe(OnModelRemoved).DisposeWithEx(_disposable);
    }

    public void Dispose() => _disposable.Dispose();

    private readonly SourceList<ModelConfig> _source = new();
    private readonly CompositeDisposable _disposable = new();

    private async void AddInitialConfigs(ConfigsDataAccess configsDataAccess)
    {
        var configs = await configsDataAccess.GetConfigs();
        _source.AddRange(configs);
    }

    private void OnModelRemoved(ModelConfig config) => _source.Remove(config);
    private void OnModelAdded(ModelConfig config) => _source.Add(config);
}