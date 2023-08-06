using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class AnnotatorViewModel : ViewModel, IAnnotatingViewModel, IActivatableViewModel
{
	public IObservable<Model?> SelectedModelChanged => _selectedModelChanged;
	public ViewModelActivator Activator { get; } = new();
	public Task<IReadOnlyCollection<Model>> Models => _modelsDataAccess.GetModels();

	public AnnotatorScreenshotsViewModel Screenshots { get; }

	public ScreenshoterViewModel Screenshoter { get; }

	public AnnotatorTools? Tools
	{
		get => _tools;
		private set => SetProperty(ref _tools, value);
	}

	public AnnotatorWorkSpace? WorkSpace
	{
		get => _workSpace;
		private set => SetProperty(ref _workSpace, value);
	}

	public bool CanChangeSelectedModel => !Screenshoter.IsEnabled;

	public AnnotatorViewModel(
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
	private readonly Subject<Model?> _selectedModelChanged = new();

	[ObservableProperty] private Model? _selectedModel;
	private Window? _window;
	private IDisposable? _selectedModelDisposable;
	private AnnotatorTools? _tools;
	private AnnotatorWorkSpace? _workSpace;

	private void HandleActivation(CompositeDisposable disposable)
	{
		_window = this.GetOwnerWindow();
		Disposable.Create(HandleDeactivation).DisposeWith(disposable);
		OnPropertyChanged(nameof(Models));
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
		if (value == null)
		{
			ClearModelEnvironment();
			return;
		}

		var selectedModelScope = _scope.BeginLifetimeScope(value);
		_selectedModelDisposable = selectedModelScope;
		if (value is DetectorModel)
			SetupDetectorModelEnvironment(selectedModelScope);
		else
			ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value), value, null);
		_selectedModelChanged.OnNext(value);
	}

	private void SetupDetectorModelEnvironment(IComponentContext content)
	{
		Tools = content.Resolve<AnnotatorTools<DetectorModel>>();
		WorkSpace = content.Resolve<AnnotatorWorkSpace<DetectorModel>>();
	}

	private void ClearModelEnvironment()
	{
		Tools = null;
		WorkSpace = null;
	}
}