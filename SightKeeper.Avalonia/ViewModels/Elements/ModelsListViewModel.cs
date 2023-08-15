using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Application.Model.Editing;
using SightKeeper.Domain.Model;
using SightKeeper.Services;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class ModelsListViewModel : ViewModel, IDisposable
{
    public ReadOnlyObservableCollection<ModelViewModel> Models { get; }

    public ModelsListViewModel(ModelsObservableRepository modelsObservableRepository, ModelEditor editor)
    {
        _disposable = new CompositeDisposable(
            modelsObservableRepository.Models.Connect()
                .Transform(model => new ModelViewModel(model))
                .AddKey(viewModel => viewModel.Model)
                .Bind(out var models)
                .PopulateInto(_cache),
            editor.ModelEdited.Subscribe(OnModelEdited));
        Models = models;
    }

    public void Dispose() => _disposable.Dispose();

    private readonly IDisposable _disposable;
    private readonly SourceCache<ModelViewModel, Model> _cache = new(viewModel => viewModel.Model);

    private void OnModelEdited(Model model) => _cache.Lookup(model).Value.NotifyChanges();
}