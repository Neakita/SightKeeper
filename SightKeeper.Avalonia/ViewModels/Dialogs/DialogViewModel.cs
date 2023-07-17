using System;
using System.Reactive;

namespace SightKeeper.Avalonia.ViewModels.Dialogs;

public interface DialogViewModel
{
    IObservable<Unit> CloseRequested { get; }
}