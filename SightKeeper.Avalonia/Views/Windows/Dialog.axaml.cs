using System;
using Avalonia.Controls;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Views.Windows;

public partial class DialogWindow : Window
{
    public DialogWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
        Closed += OnClosed;
    }

    private DialogViewModel? ViewModel
    {
        get => _viewModel;
        set
        {
            _closeRequestedSubscription?.Dispose();
            _viewModel = value;
            if (_viewModel != null)
                _closeRequestedSubscription = _viewModel.CloseRequested.Subscribe(_ => OnCloseRequested());
        }
    }
    private DialogViewModel? _viewModel;
    private IDisposable? _closeRequestedSubscription;

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        ViewModel = DataContext as DialogViewModel;
    }

    private void OnCloseRequested() => Close();

    private void OnClosed(object? sender, EventArgs e)
    {
        DataContextChanged -= OnDataContextChanged;
        Closed -= OnClosed;
        _closeRequestedSubscription?.Dispose();
    }
}