using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using NSubstitute;
using SightKeeper.Avalonia.ViewModels.Dialogs;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeWeightsEditorViewModel : IWeightsEditorViewModel
{
    public IReadOnlyCollection<Weights> Weights { get; }
    public ICommand AddWeightsFromFileCommand { get; } = Substitute.For<ICommand>();
    public ICommand CloseCommand { get; } = Substitute.For<ICommand>();

    public FakeWeightsEditorViewModel()
    {
        Weights = new[]
        {
            new Weights(Array.Empty<byte>(), ModelSize.Nano, 100, 1.234f, 0.123f, 2.43f, Array.Empty<Asset>()),
            new Weights(Array.Empty<byte>(), ModelSize.Medium, 1000, 1.2234f, 0.1123f, 2.433f, Array.Empty<Asset>())
        };
    }
    
    public Task SetLibrary(WeightsLibrary library, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}