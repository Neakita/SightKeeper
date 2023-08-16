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
	public IObservable<DataSetViewModel?> SelectedDataSetChanged => _selectedDataSetChanged;
	public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }

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

	public bool CanChangeSelectedDataSet => !Screenshoter.IsEnabled;

	public AnnotatorViewModel(
		ILifetimeScope scope,
		ScreenshoterViewModel screenshoterViewModel,
		AnnotatorScreenshotsViewModel screenshots,
		DataSetsListViewModel dataSetsListViewModel)
	{
		Screenshoter = screenshoterViewModel;
		_scope = scope;
		Screenshots = screenshots;
		screenshoterViewModel.IsEnabledChanged.Subscribe(_ =>
			OnPropertyChanged(nameof(CanChangeSelectedDataSet)));
		DataSets = dataSetsListViewModel.DataSets;
	}

	private readonly ILifetimeScope _scope;
	private readonly Subject<DataSetViewModel?> _selectedDataSetChanged = new();

	[ObservableProperty] private DataSetViewModel? _selectedDataSet;
	private IDisposable? _selectedDataSetDisposable;
	private AnnotatorTools? _tools;
	private AnnotatorWorkSpace? _workSpace;

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		_selectedDataSetDisposable?.Dispose();
		Screenshoter.DataSet = value?.DataSet;
		Screenshots.DataSet = value?.DataSet;
		if (value == null)
		{
			ClearDataSetEnvironment();
			return;
		}

		var selectedDataSetScope = _scope.BeginLifetimeScope(value);
		_selectedDataSetDisposable = selectedDataSetScope;
		if (value.DataSet is DetectorDataSet)
			SetupDetectorDataSetEnvironment(selectedDataSetScope);
		else
			ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value), value, null);
		_selectedDataSetChanged.OnNext(value);
	}

	private void SetupDetectorDataSetEnvironment(IComponentContext content)
	{
		Tools = content.Resolve<AnnotatorTools<DetectorDataSet>>();
		WorkSpace = content.Resolve<AnnotatorWorkSpace<DetectorDataSet>>();
	}

	private void ClearDataSetEnvironment()
	{
		Tools = null;
		WorkSpace = null;
	}
}