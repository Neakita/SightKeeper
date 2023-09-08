using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Commons;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class SelectedDataSetViewModel : ValueViewModel<DataSetViewModel?>
{
    private readonly WeightsDataAccess _weightsDataAccess;
    public IReadOnlyCollection<Weights> Weights { get; }

    public SelectedDataSetViewModel(WeightsDataAccess weightsDataAccess) : base(null)
    {
        _weightsDataAccess = weightsDataAccess;
        _weights.Connect()
            .Bind(out var weights)
            .Subscribe()
            .DisposeWithEx(_disposable);
        Weights = weights;
        weightsDataAccess.WeightsCreated
            .Subscribe(newWeights => _weights.Add(newWeights))
            .DisposeWithEx(_disposable);
        weightsDataAccess.WeightsDeleted
            .Subscribe(deletedWeights => _weights.Remove(deletedWeights))
            .DisposeWithEx(_disposable);
    }

    public override void Dispose()
    {
        _disposable.Dispose();
        _weights.Dispose();
        base.Dispose();
    }

    protected override void OnValueChanged(DataSetViewModel? newValue)
    {
        _weights.Clear();
        if (newValue == null)
            return;
        _weightsDataAccess.LoadWeights(newValue.DataSet.WeightsLibrary);
        _weights.AddRange(newValue.DataSet.WeightsLibrary.Weights);
    }

    private readonly SourceList<Weights> _weights = new();
    private readonly CompositeDisposable _disposable = new();
}