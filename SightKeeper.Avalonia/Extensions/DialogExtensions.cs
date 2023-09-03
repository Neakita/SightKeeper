using System.Threading.Tasks;
using SightKeeper.Avalonia.ViewModels;
using SightKeeper.Avalonia.ViewModels.Dialogs.Abstract;
using SightKeeper.Avalonia.Views.Windows;

namespace SightKeeper.Avalonia.Extensions;

public static class DialogExtensions
{
    public static Task ShowDialog(this IDialogViewModel dialogViewModel, ViewModel ownerViewModel)
    {
        DialogWindow dialogWindow = new(dialogViewModel);
        var ownerWindow = ownerViewModel.GetOwnerWindow();
        return dialogWindow.ShowDialog(ownerWindow);
    }

    public static Task<TResult> ShowDialog<TResult>(this IDialogViewModel<TResult> dialogViewModel, ViewModel ownerViewModel)
    {
        DialogWindow dialogWindow = new(dialogViewModel);
        var ownerWindow = ownerViewModel.GetOwnerWindow();
        return dialogWindow.ShowDialog<TResult>(ownerWindow);
    }
}