using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Application.DataSet.Editing;
using SightKeeper.Domain.Model;
using SightKeeper.Services;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class DataSetsListViewModel : ViewModel, IDisposable
{
    public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }

    public DataSetsListViewModel(DataSetsObservableRepository dataSetsObservableRepository, DataSetEditor editor)
    {
        _disposable = new CompositeDisposable(
            dataSetsObservableRepository.Models.Connect()
                .Transform(DataSetViewModel.Create)
                .DisposeMany()
                .AddKey(viewModel => viewModel.DataSet)
                .Bind(out var models)
                .PopulateInto(_cache),
            editor.DataSetEdited.Subscribe(OnDataSetEdited));
        DataSets = models;
    }

    public void Dispose() => _disposable.Dispose();

    private readonly IDisposable _disposable;
    private readonly SourceCache<DataSetViewModel, DataSet> _cache = new(viewModel => viewModel.DataSet);

    private void OnDataSetEdited(DataSet dataSet) => _cache.Lookup(dataSet).Value.NotifyDataSetEdited();
}