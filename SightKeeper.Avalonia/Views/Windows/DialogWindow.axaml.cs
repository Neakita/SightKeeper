using System;
using System.Reactive.Linq;
using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Dialogs.Abstract;

namespace SightKeeper.Avalonia.Views.Windows;

public sealed partial class DialogWindow : ReactiveWindow<IDialogViewModel>
{
    public DialogWindow(IDialogViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        ViewModel.Result.Take(1).Subscribe(Close);
    }
}