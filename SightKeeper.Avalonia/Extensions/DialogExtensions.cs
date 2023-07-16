using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.VisualTree;
using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.ViewModels;
using SightKeeper.Avalonia.Views.Windows;

namespace SightKeeper.Avalonia.Extensions;

public static class DialogExtensions
{
    public static async Task ShowDialog(this DialogViewModel viewModel, ViewModel owner)
    {
        DialogWindow dialog = new();
        dialog.DataContext = viewModel;
        await dialog.ShowDialog(GetOwnerWindow(owner));
    }

    private static Window GetOwnerWindow(ViewModel dataContext)
    {
        var firstWithDataContext = GetWindows().SelectMany(window => window.GetVisualChildrenRecursive().Prepend(window))
            .OfType<StyledElement>().FirstOrDefault(element => element.DataContext == dataContext) as Visual;
        Guard.IsNotNull(firstWithDataContext);
        return (Window)TopLevel.GetTopLevel(firstWithDataContext)!;
    }

    private static IEnumerable<Window> GetWindows()
    {
        var lifetime = (ClassicDesktopStyleApplicationLifetime)global::Avalonia.Application.Current!.ApplicationLifetime!;
        return lifetime.Windows;
    }

    private static IEnumerable<Visual> GetVisualChildrenRecursive(this Visual visual)
    {
        var children = visual.GetVisualChildren();
        return children.SelectMany(child => GetVisualChildrenRecursive(child).Prepend(child));
    }
}