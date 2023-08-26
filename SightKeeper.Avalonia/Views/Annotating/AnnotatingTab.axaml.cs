using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using CommunityToolkit.Diagnostics;
using ReactiveUI;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Annotating;

namespace SightKeeper.Avalonia.Views.Annotating;

public sealed partial class AnnotatingTab : ReactiveUserControl<AnnotatorViewModel>
{
	public AnnotatingTab()
	{
		this.WhenActivated(OnActivated);
		InitializeComponent();
	}

	private TopLevel? _topLevel;
	private bool _scrollScreenshotsInsteadOfItemClasses;

	private void OnActivated(CompositeDisposable disposable)
	{
		Disposable.Create(OnDeactivated).DisposeWith(disposable);
		_topLevel = this.GetTopLevel();
		_topLevel.KeyDown += OnTopLevelKeyDown;
		_topLevel.KeyUp += OnTopLevelKeyUp;
		_topLevel.PointerWheelChanged += OnTopLevelScrolled;
	}

	private void OnDeactivated()
	{
		Guard.IsNotNull(_topLevel);
		_topLevel.KeyDown -= OnTopLevelKeyDown;
		_topLevel.KeyUp -= OnTopLevelKeyUp;
		_topLevel.PointerWheelChanged -= OnTopLevelScrolled;
	}

	private void OnTopLevelKeyDown(object? sender, KeyEventArgs e)
	{
		if (e.Key == Key.LeftShift)
			_scrollScreenshotsInsteadOfItemClasses = true;
	}

	private void OnTopLevelKeyUp(object? sender, KeyEventArgs e)
	{
		if (e.Key == Key.LeftShift)
			_scrollScreenshotsInsteadOfItemClasses = false;
	}

	private void OnTopLevelScrolled(object? sender, PointerWheelEventArgs e)
	{
		var delta = e.Delta.Y;
		if (delta == 0)
			return;
		var reverse = delta > 0;
		if (_scrollScreenshotsInsteadOfItemClasses)
			ScrollScreenshot(reverse);
		else
			ScrollItemClass(reverse);
	}

	private void ScrollItemClass(bool reverse) =>
		ViewModel?.ToolsViewModel.ScrollItemClass(reverse);

	private void ScrollScreenshot(bool reverse) =>
		ViewModel?.Screenshots.ScrollScreenshot(reverse);
}