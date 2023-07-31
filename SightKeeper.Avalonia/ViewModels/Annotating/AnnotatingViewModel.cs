using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class AnnotatingViewModel : ViewModel, IAnnotatingViewModel, IActivatableViewModel
{
	public ViewModelActivator Activator { get; } = new();
	public Task<IReadOnlyCollection<Model>> Models => _modelsDataAccess.GetModels();

	public AnnotatorScreenshotsViewModel Screenshots { get; }

	public ScreenshoterViewModel Screenshoter { get; }
	public bool CanChangeSelectedModel => !Screenshoter.IsEnabled;

	public AnnotatingViewModel(
		ScreenshoterViewModel screenshoterViewModel,
		ModelsDataAccess modelsDataAccess,
		AnnotatorScreenshotsViewModel screenshots)
	{
		Screenshoter = screenshoterViewModel;
		_modelsDataAccess = modelsDataAccess;
		Screenshots = screenshots;
		this.WhenActivated(HandleActivation);
		screenshoterViewModel.IsEnabledChanged.Subscribe(_ =>
			OnPropertyChanged(nameof(CanChangeSelectedModel)));
	}

	private readonly ModelsDataAccess _modelsDataAccess;

	[ObservableProperty] private Model? _selectedModel;
	private TopLevel? _topLevel;

	private IObservable<Unit> TopLevelGotFocus => Observable.FromEventPattern<GotFocusEventArgs>(
		handler =>
		{
			Guard.IsNotNull(_topLevel);
			_topLevel.GotFocus += handler;
		}, handler =>
		{
			Guard.IsNotNull(_topLevel);
			_topLevel.GotFocus -= handler;
		}).Select(_ => Unit.Default);

	private IObservable<Unit> TopLevelLostFocus => Observable.FromEventPattern<RoutedEventArgs>(
		handler =>
		{
			Guard.IsNotNull(_topLevel);
			_topLevel.LostFocus += handler;
		}, handler =>
		{
			Guard.IsNotNull(_topLevel);
			_topLevel.LostFocus -= handler;
		}).Select(_ => Unit.Default);

	private void HandleActivation(CompositeDisposable disposable)
	{
		_topLevel = this.GetTopLevel();
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
		Screenshots.Model = value;
	}
}