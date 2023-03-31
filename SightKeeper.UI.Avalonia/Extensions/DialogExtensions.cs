using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Material.Icons;
using ReactiveUI;
using SightKeeper.Infrastructure.Common;
using SightKeeper.UI.Avalonia.Misc;
using SightKeeper.UI.Avalonia.Views.Windows;

namespace SightKeeper.UI.Avalonia.Extensions;

public static class DialogExtensions
{
	public static async Task<TResult> ShowDialog<TResult, TCallerViewModel>(this IViewFor<TCallerViewModel> callerView, Dialog<TResult> dialog) where TCallerViewModel : class
	{
		IControl control = (IControl)callerView;
		Window dialogOwner = control as Window ?? control.GetParentWindow();
		return await dialog.ShowDialog(dialogOwner);
	}
	
	public static async Task<TResult> ShowDialog<TResult, TCallerViewModel>(this TCallerViewModel callerViewModel, Dialog<TResult> dialog) where TCallerViewModel : class
	{
		var callerView = Locator.Resolve<IViewFor<TCallerViewModel>>();
		if (callerView.ViewModel != callerViewModel) throw new Exception($"Incorrect View Instance ({callerView}) for {callerViewModel.GetType()} received from {nameof(Locator)}");
		return await callerView.ShowDialog(dialog);
	}

	public static async Task<MessageBoxDialog.DialogResult> ShowMessageBoxDialog<TCallerViewModel>(this IViewFor<TCallerViewModel> callerView, string message, MessageBoxDialog.DialogResult dialogResults = MessageBoxDialog.DialogResult.Ok, string title = "", MaterialIconKind? icon = null) where TCallerViewModel : class
	{
		MessageBoxDialog dialog = new(message, dialogResults, title, icon);
		return await callerView.ShowDialog(dialog);
	}
	
	public static async Task<MessageBoxDialog.DialogResult> ShowMessageBoxDialog<TCallerViewModel>(this TCallerViewModel callerViewModel, string message, MessageBoxDialog.DialogResult dialogResults = MessageBoxDialog.DialogResult.Ok, string title = "", MaterialIconKind? icon = null) where TCallerViewModel : class
	{
		MessageBoxDialog dialog = new(message, dialogResults, title, icon);
		return await callerViewModel.ShowDialog(dialog);
	}
}