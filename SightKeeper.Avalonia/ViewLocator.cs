using Autofac;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Diagnostics;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia;

public sealed class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is not ViewModel) ThrowHelper.ThrowArgumentException(nameof(param), $"Must be {nameof(ViewModel)}");
        var view = (Control)ServiceLocator.Instance.Resolve(typeof(IViewFor<>).MakeGenericType(param.GetType()));
        view.DataContext = param;
        return view;
    }

    public bool Match(object? data) =>
        data is ViewModel && ServiceLocator.Instance.IsRegistered(typeof(IViewFor<>).MakeGenericType(data.GetType()));
}