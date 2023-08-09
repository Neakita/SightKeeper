using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Application.Model.Creating;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services;

public sealed class ModelsObservableRepository : IDisposable
{
    public IObservableList<Domain.Model.Model> ModelsObservableList => _modelsSource;

    public ModelsObservableRepository(ModelCreator modelCreator, ModelsDataAccess modelsDataAccess)
    {
        _modelsDataAccess = modelsDataAccess;
        _disposable = new CompositeDisposable(
            modelCreator.ModelCreated.Subscribe(OnModelCreated),
            modelsDataAccess.ModelRemoved.Subscribe(OnModelRemoved));
        _ = AddInitialModels();
    }

    public void Dispose()
    {
        _modelsSource.Dispose();
        _disposable.Dispose();
    }

    private readonly SourceList<Domain.Model.Model> _modelsSource = new();
    private readonly IDisposable _disposable;
    private readonly ModelsDataAccess _modelsDataAccess;

    private void OnModelCreated(Domain.Model.Model model) => _modelsSource.Add(model);
    private void OnModelRemoved(Domain.Model.Model model) => _modelsSource.Remove(model);

    private async Task AddInitialModels()
    {
        var models = await _modelsDataAccess.GetModels();
        _modelsSource.AddRange(models);
    }
}