using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Application.Model.Creating;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services;

public sealed class ModelsObservableRepository : IDisposable
{
    public IObservableList<Domain.Model.Model> Models => _source;

    public ModelsObservableRepository(ModelCreator modelCreator, ModelsDataAccess modelsDataAccess)
    {
        _disposable = new CompositeDisposable(
            modelCreator.ModelCreated.Subscribe(OnModelCreated),
            modelsDataAccess.ModelRemoved.Subscribe(OnModelRemoved));
        AddInitialModels(modelsDataAccess);
    }

    public void Dispose()
    {
        _source.Dispose();
        _disposable.Dispose();
    }

    private readonly SourceList<Domain.Model.Model> _source = new();
    private readonly IDisposable _disposable;

    private void OnModelCreated(Domain.Model.Model model) => _source.Add(model);
    private void OnModelRemoved(Domain.Model.Model model) => _source.Remove(model);

    private async void AddInitialModels(ModelsDataAccess modelsDataAccess)
    {
        var models = await modelsDataAccess.GetModels();
        _source.AddRange(models);
    }
}