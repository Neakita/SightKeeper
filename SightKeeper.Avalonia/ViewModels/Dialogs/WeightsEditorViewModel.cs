using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Dialogs;

public sealed partial class WeightsEditorViewModel : ViewModel, IWeightsEditorViewModel, DialogViewModel
{
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

    private readonly WeightsDataAccess _weightsDataAccess;
    private readonly SourceList<Weights> _weightsSource = new();
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(DeleteSelectedWeightsCommand))] private Weights? _selectedWeights;

    ICommand IWeightsEditorViewModel.DeleteSelectedWeightsCommand => DeleteSelectedWeightsCommand;
    [RelayCommand(CanExecute = nameof(CanDeleteSelectedWeights))]
    private async Task DeleteSelectedWeights(CancellationToken cancellationToken)
    {
        Guard.IsNotNull(SelectedWeights);
        await _weightsDataAccess.DeleteWeights(SelectedWeights, cancellationToken);
        _weightsSource.Remove(SelectedWeights);
    }

    private bool CanDeleteSelectedWeights() => SelectedWeights != null;
    public IObservable<Unit> CloseRequested { get; } = Observable.Empty<Unit>();
}