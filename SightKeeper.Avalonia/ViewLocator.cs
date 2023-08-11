using System;
using Autofac;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Diagnostics;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia;

public sealed class ViewLocator : IDataTemplate
{
    public Control Build(object? param)
    {
        Guard.IsNotNull(param);
        Guard.IsAssignableToType<ViewModel>(param);
        var type = GetViewTypeFor(param);
        var view = (Control)ServiceLocator.Instance.Resolve(type);
        view.DataContext = param;
        return view;
    }

    public bool Match(object? data) =>
        data is ViewModel && ServiceLocator.Instance.IsRegistered(GetViewTypeFor(data));

    private static Type GetViewTypeFor(object data) => typeof(IViewFor<>).MakeGenericType(data.GetType());
}