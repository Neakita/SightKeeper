using System.Threading.Tasks;
using SightKeeper.Avalonia.ViewModels;
using SightKeeper.Avalonia.ViewModels.Dialogs;
using SightKeeper.Avalonia.Views.Windows;

namespace SightKeeper.Avalonia.Extensions;

public static class DialogExtensions
{
    public static async Task ShowDialog(this DialogViewModel viewModel, ViewModel owner)
    {
        DialogWindow dialog = new();
        dialog.DataContext = viewModel;
        await dialog.ShowDialog(owner.GetOwnerWindow());
    }
}