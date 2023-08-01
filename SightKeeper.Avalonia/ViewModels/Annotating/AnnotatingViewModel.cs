using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Annotating.AnnotatorTools;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class AnnotatingViewModel : ViewModel, IAnnotatingViewModel, IActivatableViewModel
{
	public ViewModelActivator Activator { get; } = new();
	public Task<IReadOnlyCollection<Model>> Models => _modelsDataAccess.GetModels();

	public AnnotatorScreenshotsViewModel Screenshots { get; }

	public ScreenshoterViewModel Screenshoter { get; }

	public AnnotatorTools.AnnotatorTools? Tools
	{
		get => _tools;
		private set => SetProperty(ref _tools, value);
	}

	public bool CanChangeSelectedModel => !Screenshoter.IsEnabled;

	public AnnotatingViewModel(
		ILifetimeScope scope,
		ScreenshoterViewModel screenshoterViewModel,
		ModelsDataAccess modelsDataAccess,
		AnnotatorScreenshotsViewModel screenshots)
	{
		Screenshoter = screenshoterViewModel;
		_scope = scope;
		_modelsDataAccess = modelsDataAccess;
		Screenshots = screenshots;
		this.WhenActivated(HandleActivation);
		screenshoterViewModel.IsEnabledChanged.Subscribe(_ =>
			OnPropertyChanged(nameof(CanChangeSelectedModel)));
	}

	private readonly ILifetimeScope _scope;
	private readonly ModelsDataAccess _modelsDataAccess;

	[ObservableProperty] private Model? _selectedModel;
	private TopLevel? _topLevel;
	private IDisposable? _selectedModelDisposable;
	private AnnotatorTools.AnnotatorTools? _tools;

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
		_selectedModelDisposable?.Dispose();
		Screenshoter.Model = value;
		Screenshots.Model = value;
		if (value == null) return;
		var selectedModelScope = _scope.BeginLifetimeScope(value);
		_selectedModelDisposable = selectedModelScope;
		Tools = value switch
		{
			DetectorModel detectorModel => selectedModelScope.Resolve<AnnotatorTools<DetectorModel>>(new PositionalParameter(0, detectorModel)),
			_ => ThrowHelper.ThrowArgumentOutOfRangeException<AnnotatorTools.AnnotatorTools>(nameof(value), value, null)
		};
	}
}