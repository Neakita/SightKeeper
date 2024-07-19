using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Extensions;

namespace SightKeeper.Avalonia.Views.Annotating;

internal sealed partial class AnnotatingTab : UserControl
{
	public AnnotatingTab()
	{
		throw new NotImplementedException();
		// this.WhenActivated(OnActivated);
		// InitializeComponent();
	}

	private TopLevel? _topLevel;
	// private bool _scrollScreenshotsInsteadOfTags;

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
		throw new NotImplementedException();
		// if (e.Key == Key.LeftShift)
		// 	_scrollScreenshotsInsteadOfTags = true;
	}

	private void OnTopLevelKeyUp(object? sender, KeyEventArgs e)
	{
		throw new NotImplementedException();
		// if (e.Key == Key.LeftShift)
		// 	_scrollScreenshotsInsteadOfTags = false;
	}

	private void OnTopLevelScrolled(object? sender, PointerWheelEventArgs e)
	{
		throw new NotImplementedException();
		// var delta = e.Delta.Y;
		// if (delta == 0)
		// 	return;
		// var reverse = delta > 0;
		// if (_scrollScreenshotsInsteadOfTags)
		// 	ScrollScreenshot(reverse);
		// else
		// 	ScrollTag(reverse);
	}

	/*private void ScrollTag(bool reverse) =>
		ViewModel?.ToolsViewModel.ScrollTag(reverse);

	private void ScrollScreenshot(bool reverse) =>
		ViewModel?.Screenshots.ScrollScreenshot(reverse);*/
}