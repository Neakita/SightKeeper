using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Dialogs;

public interface IWeightsEditorViewModel
{
    IReadOnlyCollection<Weights> Weights { get; }
    ICommand CloseCommand { get; }
    Task SetLibrary(WeightsLibrary library, CancellationToken cancellationToken = default);
}