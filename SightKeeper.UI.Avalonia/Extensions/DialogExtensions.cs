using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using SightKeeper.Infrastructure.Common;
using SightKeeper.UI.Avalonia.Misc;

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
}
