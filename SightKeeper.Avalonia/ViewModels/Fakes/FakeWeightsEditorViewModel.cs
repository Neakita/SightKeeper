using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using NSubstitute;
using SightKeeper.Avalonia.ViewModels.Dialogs;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeWeightsEditorViewModel : IWeightsEditorViewModel
{
    public IReadOnlyCollection<Weights> Weights { get; }
    public Weights? SelectedWeights { get; set; }
    public ICommand DeleteSelectedWeightsCommand { get; } = Substitute.For<ICommand>();

    public FakeWeightsEditorViewModel()
    {
        DataSet dataSet = new("Mock data set");
        dataSet.Weights.CreateWeights(Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Nano, new WeightsMetrics(100, 1.234f, 0.123f, 2.43f), dataSet.ItemClasses);
        dataSet.Weights.CreateWeights(Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Medium, new WeightsMetrics(1000, 1.2234f, 0.1123f, 2.433f), dataSet.ItemClasses);
        Weights = dataSet.Weights.Records;
    }

    public Task SetLibrary(WeightsLibrary weightsLibrary, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}