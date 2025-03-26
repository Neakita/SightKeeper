using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Avalonia.DataSets;
using Vibrance;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed partial class SideBarViewModel : ViewModel, AnnotationSideBarComponent
{
	private readonly ToolingViewModelFactory _toolingViewModelFactory;
	public IReadOnlyCollection<ImageSetViewModel> ImageSets { get; }
	public ReadOnlyObservableList<DataSetViewModel> DataSets { get; }
	[ObservableProperty] public partial DataSetViewModel? SelectedDataSet { get; set; }
	[ObservableProperty] public partial ImageSetViewModel? SelectedImageSet { get; set; }
	public IObservable<ushort> PendingImagesCount { get; }
	public ScreenCapturingSettingsViewModel ScreenCapturingSettings { get; }
	public IObservable<ImageSetViewModel?> SelectedImageSetChanged => _selectedImageSetChanged.AsObservable();
	public IObservable<DataSetViewModel?> SelectedDataSetChanged => _selectedDataSetChanged.AsObservable();
	[ObservableProperty] public partial object? AdditionalTooling { get; private set; }
	public IObservable<object?> AdditionalToolingChanged => _additionalToolingChanged.AsObservable();

	public SideBarViewModel(
		DataSetViewModelsObservableRepository dataSetsRepository,
		ScreenCapturingSettingsViewModel screenCapturingSettings,
		PendingImagesCountReporter? pendingImagesReporter,
		ImageSetViewModelsObservableRepository imageSets,
		ToolingViewModelFactory toolingViewModelFactory)
	{
		_toolingViewModelFactory = toolingViewModelFactory;
		DataSets = dataSetsRepository.Items;
		
		ScreenCapturingSettings = screenCapturingSettings;
		PendingImagesCount = pendingImagesReporter?.PendingImagesCount ?? Observable.Empty<ushort>();
		ImageSets = imageSets.Items;
	}

	partial void OnSelectedImageSetChanged(ImageSetViewModel? value)
	{
		_selectedImageSetChanged.OnNext(value);
		ScreenCapturingSettings.Set = value?.Value;
	}

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		_selectedDataSetChanged.OnNext(value);
		AdditionalTooling = _toolingViewModelFactory.CreateToolingViewModel(value?.Value);
	}

	partial void OnAdditionalToolingChanged(object? value)
	{
		_additionalToolingChanged.OnNext(value);
	}

	private readonly Subject<ImageSetViewModel?> _selectedImageSetChanged = new();
	private readonly Subject<DataSetViewModel?> _selectedDataSetChanged = new();
	private readonly Subject<object?> _additionalToolingChanged = new();
}