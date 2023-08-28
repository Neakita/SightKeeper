using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
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
        _library = library;
        _weightsSource.Clear();
        _weightsSource.AddRange(library.Weights);
    }

    private static readonly FilePickerOpenOptions FilePickerOptions = new()
    {
        Title = "Choose ONNX model file"
    };

    private readonly Subject<Unit> _closeRequested = new();
    private readonly WeightsDataAccess _weightsDataAccess;
    private readonly SourceList<Weights> _weightsSource = new();
    private WeightsLibrary? _library;

    ICommand IWeightsEditorViewModel.AddWeightsFromFileCommand => AddWeightsFromFileCommand;
    [RelayCommand]
    private async Task AddWeightsFromFile(CancellationToken cancellationToken)
    {
        Guard.IsNotNull(_library);
        var dialogResult = await this.GetTopLevel().StorageProvider.OpenFilePickerAsync(FilePickerOptions);
        var file = dialogResult.SingleOrDefault();
        if (file == null)
            return;
        var filePath = file.Path.AbsolutePath;
        var fileContent = await File.ReadAllBytesAsync(filePath, cancellationToken);
        var weights = await _weightsDataAccess.CreateWeights(_library, fileContent, ModelSize.Nano, 0, 0, 0, 0, Array.Empty<Asset>(),
            cancellationToken);
        _weightsSource.Add(weights);
    }

    ICommand IWeightsEditorViewModel.CloseCommand => CloseCommand;
    [RelayCommand]
    private void Close() => _closeRequested.OnNext(Unit.Default);
}