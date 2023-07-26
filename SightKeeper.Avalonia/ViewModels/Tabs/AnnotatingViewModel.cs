using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using SightKeeper.Application.Annotating;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class AnnotatingViewModel : ViewModel, IAnnotatingViewModel, IActivatableViewModel
{
	public ViewModelActivator Activator { get; } = new();
	public Task<IReadOnlyCollection<Model>> Models => _modelsDataAccess.GetModels();

	public IReadOnlyCollection<Screenshot> Screenshots { get; }

	public ScreenshoterViewModel Screenshoter { get; }
	public bool CanChangeSelectedModel => !Screenshoter.IsEnabled;

	public AnnotatingViewModel(Screenshoter screenshoter, ScreenshoterViewModel screenshoterViewModel, ModelsDataAccess modelsDataAccess)
	{
		Screenshoter = screenshoterViewModel;
		_modelsDataAccess = modelsDataAccess;
		this.WhenActivated(HandleActivation);
		screenshoter.Screenshoted.Merge(screenshoter.ScreenshotRemoved).Subscribe(_ => Dispatcher.UIThread.Invoke(() => OnPropertyChanged(nameof(Screenshots))));
		_screenshots.Connect()
			.Sort(SortExpressionComparer<Screenshot>.Descending(screenshot => screenshot.CreationDate))
			.ObserveOn(RxApp.MainThreadScheduler)
			.Bind(out var screenshots)
			.Subscribe();
		Screenshots = screenshots;
		screenshoter.Screenshoted.Subscribe(newScreenshot => _screenshots.Add(newScreenshot));
		screenshoter.ScreenshotRemoved.Subscribe(removedScreenshot => _screenshots.Remove(removedScreenshot));
		screenshoterViewModel.IsEnabledChanged.Subscribe(_ =>
			OnPropertyChanged(nameof(CanChangeSelectedModel)));
	}

	private readonly ModelsDataAccess _modelsDataAccess;
	
	[ObservableProperty] private Model? _selectedModel;
	private readonly SourceList<Screenshot> _screenshots = new();

	private IObservable<Unit> TopLevelGotFocus => Observable.FromEventPattern<GotFocusEventArgs>(
		handler => this.GetTopLevel().GotFocus += handler, handler => this.GetTopLevel().GotFocus -= handler).Select(_ => Unit.Default);

	private IObservable<Unit> TopLevelLostFocus => Observable.FromEventPattern<RoutedEventArgs>(
		handler => this.GetTopLevel().LostFocus += handler, handler => this.GetTopLevel().LostFocus -= handler).Select(_ => Unit.Default);

	private void HandleActivation(CompositeDisposable disposable)
	{
		Disposable.Create(HandleDeactivation).DisposeWith(disposable);
		OnPropertyChanged(nameof(Models));
		TopLevelGotFocus.Subscribe(_ => Screenshoter.IsSuspended = true).DisposeWith(disposable);
		TopLevelLostFocus.Subscribe(_ => Screenshoter.IsSuspended = false).DisposeWith(disposable);
	}

	private void HandleDeactivation()
	{
		Screenshoter.IsEnabled = false;
		SelectedModel = null;
	}

	partial void OnSelectedModelChanged(Model? value)
	{
		Screenshoter.Model = value;
		_screenshots.Clear();
		if (value != null)
			_screenshots.AddRange(value.ScreenshotsLibrary.Screenshots);
	}
}