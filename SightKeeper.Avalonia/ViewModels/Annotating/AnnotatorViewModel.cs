using System;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using Autofac;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class AnnotatorViewModel : ViewModel, IAnnotatingViewModel
{
	public IObservable<ModelViewModel?> SelectedModelChanged => _selectedModelChanged;
	public ReadOnlyObservableCollection<ModelViewModel> Models { get; }

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
		AnnotatorScreenshotsViewModel screenshots,
		ModelsListViewModel modelsListViewModel)
	{
		Screenshoter = screenshoterViewModel;
		_scope = scope;
		Screenshots = screenshots;
		screenshoterViewModel.IsEnabledChanged.Subscribe(_ =>
			OnPropertyChanged(nameof(CanChangeSelectedModel)));
		Models = modelsListViewModel.Models;
	}

	private readonly ILifetimeScope _scope;
	private readonly Subject<ModelViewModel?> _selectedModelChanged = new();

	[ObservableProperty] private ModelViewModel? _selectedModel;
	private IDisposable? _selectedModelDisposable;
	private AnnotatorTools? _tools;
	private AnnotatorWorkSpace? _workSpace;

	partial void OnSelectedModelChanged(ModelViewModel? value)
	{
		_selectedModelDisposable?.Dispose();
		Screenshoter.Model = value?.DataSet;
		Screenshots.Model = value?.DataSet;
		if (value == null)
		{
			ClearModelEnvironment();
			return;
		}

		var selectedModelScope = _scope.BeginLifetimeScope(value);
		_selectedModelDisposable = selectedModelScope;
		if (value.DataSet is DetectorDataSet)
			SetupDetectorModelEnvironment(selectedModelScope);
		else
			ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value), value, null);
		_selectedModelChanged.OnNext(value);
	}

	private void SetupDetectorModelEnvironment(IComponentContext content)
	{
		Tools = content.Resolve<AnnotatorTools<DetectorDataSet>>();
		WorkSpace = content.Resolve<AnnotatorWorkSpace<DetectorDataSet>>();
	}

	private void ClearModelEnvironment()
	{
		Tools = null;
		WorkSpace = null;
	}
}