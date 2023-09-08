using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Application.DataSet.Editing;
using SightKeeper.Commons;
using SightKeeper.Domain.Model;
using SightKeeper.Services;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class DataSetsListViewModel : ViewModel, IDisposable
{
    public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }

    public DataSetsListViewModel(DataSetsObservableRepository dataSetsObservableRepository, DataSetEditor editor)
    {
        dataSetsObservableRepository.DataSets.Connect()
            .Transform(dataSet => new DataSetViewModel(dataSet))
            .DisposeMany()
            .AddKey(viewModel => viewModel.DataSet)
            .Bind(out var dataSets)
            .PopulateInto(_cache)
            .DisposeWithEx(_disposable);
        editor.DataSetEdited
            .Subscribe(OnDataSetEdited)
            .DisposeWithEx(_disposable);
        DataSets = dataSets;
    }

    public void Dispose() => _disposable.Dispose();

    private readonly CompositeDisposable _disposable = new();
    private readonly SourceCache<DataSetViewModel, DataSet> _cache = new(viewModel => viewModel.DataSet);

    private void OnDataSetEdited(DataSet dataSet) => _cache.Lookup(dataSet).Value.NotifyChanges();
}