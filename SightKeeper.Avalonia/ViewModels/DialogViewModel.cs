using System;
using System.Reactive;

namespace SightKeeper.Avalonia.ViewModels;

public interface DialogViewModel
{
    IObservable<Unit> CloseRequested { get; }
}