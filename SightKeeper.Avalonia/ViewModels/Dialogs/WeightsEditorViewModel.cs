using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Dialogs;

public sealed partial class WeightsEditorViewModel : ViewModel, IWeightsEditorViewModel, DialogViewModel
{
    public IObservable<Unit> CloseRequested => _closeRequested.AsObservable();
    public IReadOnlyCollection<Weights> Weights { get; }

    public WeightsEditorViewModel(WeightsDataAccess weightsDataAccess)
    {
        _weightsDataAccess = weightsDataAccess;
        _weightsSource.Connect()
            .Bind(out var weights)
            .Subscribe();
        Weights = weights;
    }

    public async Task SetLibrary(WeightsLibrary library, CancellationToken cancellationToken = default)
    {
        await _weightsDataAccess.LoadWeights(library, cancellationToken);
        _weightsSource.Clear();
        _weightsSource.AddRange(library.Weights);
    }

    private readonly Subject<Unit> _closeRequested = new();
    private readonly WeightsDataAccess _weightsDataAccess;
    private readonly SourceList<Weights> _weightsSource = new();

    ICommand IWeightsEditorViewModel.CloseCommand => CloseCommand;
    [RelayCommand]
    private void Close() => _closeRequested.OnNext(Unit.Default);
}