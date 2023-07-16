using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Extensions;

public static class ViewModelExtensions
{
    public static Window GetOwnerWindow(this ViewModel viewModel) =>
        (Window)TopLevel.GetTopLevel((Visual)viewModel.GetView())!;

    public static StyledElement GetView(this ViewModel viewModel)
    {
        var firstWithDataContext = GetWindows().SelectMany(window => window.GetVisualChildrenRecursive().Prepend(window))
            .OfType<StyledElement>().FirstOrDefault(element => element.DataContext == viewModel);
        Guard.IsNotNull(firstWithDataContext);
        return firstWithDataContext;
    }

    public static TopLevel GetTopLevel(this ViewModel viewModel) =>
        TopLevel.GetTopLevel((Visual?)viewModel.GetView())!;

    private static IEnumerable<Window> GetWindows()
    {
        var lifetime = (ClassicDesktopStyleApplicationLifetime)global::Avalonia.Application.Current!.ApplicationLifetime!;
        return lifetime.Windows;
    }
}