using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.DataSets;

internal sealed class DataSetsListViewModel : ViewModel, IDisposable
{
    public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }

    public DataSetsListViewModel(ObservableRepository<DataSet> observableRepository)
    {
        observableRepository.DataSetsSource.Connect()
            .Transform(dataSet => new DataSetViewModel(dataSet))
            .DisposeMany()
            .AddKey(viewModel => viewModel.DataSet)
            .Bind(out var dataSets)
            .Subscribe()
            .DisposeWith(_disposable);
        DataSets = dataSets;
    }

    public void Dispose()
    {
	    _disposable.Dispose();
    }

    private readonly CompositeDisposable _disposable = new();
}