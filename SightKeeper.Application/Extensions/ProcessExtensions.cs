using System.Diagnostics;
using System.Reactive.Linq;

namespace SightKeeper.Application.Extensions;

public static class ProcessExtensions
{
    public static IObservable<string?> ObserveDataReceived(this Process process) =>
        process.ObserveOutputDataReceived().Merge(process.ObserveErrorDataReceived());

    public static IObservable<string?> ObserveOutputDataReceived(this Process process) => Observable
        .FromEventPattern<DataReceivedEventHandler, DataReceivedEventArgs>(
            handler => process.OutputDataReceived += handler,
            handler => process.OutputDataReceived -= handler)
        .TakeUntil(Observable.FromEventPattern(
            handler => process.Exited += handler,
            handler => process.Exited -= handler))
        .Select(args => args.EventArgs.Data);
    
    public static IObservable<string?> ObserveErrorDataReceived(this Process process) => Observable
        .FromEventPattern<DataReceivedEventHandler, DataReceivedEventArgs>(
            handler => process.ErrorDataReceived += handler,
            handler => process.ErrorDataReceived -= handler)
        .TakeUntil(Observable.FromEventPattern(
            handler => process.Exited += handler,
            handler => process.Exited -= handler))
        .Select(args => args.EventArgs.Data);
}