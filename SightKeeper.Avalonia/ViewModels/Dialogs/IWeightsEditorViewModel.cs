using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Dialogs;

public interface IWeightsEditorViewModel
{
    IReadOnlyCollection<Weights> Weights { get; }
    Weights? SelectedWeights { get; set; }
    ICommand DeleteSelectedWeightsCommand { get; }
    Task SetLibrary(WeightsLibrary weightsLibrary, CancellationToken cancellationToken = default);
}