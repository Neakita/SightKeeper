using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

internal sealed class SelectedDataSetViewModel : ValueViewModel<DataSetViewModel?>
{
    public IReadOnlyCollection<Weights> Weights { get; }

    public SelectedDataSetViewModel(WeightsDataAccess weightsDataAccess) : base(null)
    {
	    throw new NotImplementedException();
	    // _weights.Connect()
	    //     .Bind(out var weights)
	    //     .Subscribe()
	    //     .DisposeWith(_disposable);
	    // Weights = weights;
	    // weightsDataAccess.WeightsCreated
	    //     .Subscribe(data => _weights.Add(data.weights))
	    //     .DisposeWith(_disposable);
	    // weightsDataAccess.WeightsRemoved
	    //     .Subscribe(deletedWeights => _weights.Remove(deletedWeights))
	    //     .DisposeWith(_disposable);
    }

    public override void Dispose()
    {
        _disposable.Dispose();
        _weights.Dispose();
        base.Dispose();
    }

    protected override void OnValueNotified(DataSetViewModel? newValue)
    {
        _weights.Clear();
        if (newValue == null)
            return;
        // TODO do it asynchronously somehow
        _weights.AddRange(newValue.DataSet.Weights);
    }

    private readonly SourceList<Weights> _weights = new();
    private readonly CompositeDisposable _disposable = new();
}