using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Annotation.Tooling.Commands;
using SightKeeper.Avalonia.DataSets;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed partial class SideBarViewModel : ViewModel, AnnotationSideBarComponent, IDisposable
{
	public IReadOnlyCollection<ImageSetViewModel> ImageSets { get; }
	[ObservableProperty] public partial ImageSetViewModel? SelectedImageSet { get; set; }
	public IObservable<ImageSetViewModel?> SelectedImageSetChanged => _selectedImageSetChanged.AsObservable();
	public IReadOnlyCollection<AnnotationButtonDefinition> ButtonDefinitions => _buttonDefinitions;
	[ObservableProperty] public partial object? AdditionalTooling { get; private set; }
	public IObservable<object?> AdditionalToolingChanged => _additionalToolingChanged.AsObservable();

	IReadOnlyCollection<ImageSetDataContext> SideBarDataContext.ImageSets => ImageSets;

	ImageSetDataContext? SideBarDataContext.SelectedImageSet
	{
		get => SelectedImageSet;
		set => SelectedImageSet = (ImageSetViewModel?)value;
	}

	public DataSetSelectionDataContext DataSetSelection => _dataSetSelection;

	IObservable<DataSetViewModel?> AnnotationSideBarComponent.SelectedDataSetChanged =>
		_dataSetSelection.SelectedDataSetChanged;

	public SideBarViewModel(
		ImageSetViewModelsObservableRepository imageSets,
		ToolingViewModelFactory toolingViewModelFactory,
		IEnumerable<AnnotationButtonDefinitionFactory> buttonDefinitionFactories,
		DataSetSelectionViewModel dataSetSelection)
	{
		_toolingViewModelFactory = toolingViewModelFactory;
		_dataSetSelection = dataSetSelection;
		ImageSets = imageSets.Items;
		_buttonDefinitions.AddRange(buttonDefinitionFactories.Select(factory => factory.CreateButtonDefinition()));
		_selectedDataSetChangedSubscription = _dataSetSelection.SelectedDataSetChanged.Subscribe(OnSelectedDataSetChanged);
	}

	partial void OnSelectedImageSetChanged(ImageSetViewModel? value)
	{
		_selectedImageSetChanged.OnNext(value);
	}

	private void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		if (AdditionalTooling is AnnotationButtonDefinitionsProvider oldButtonDefinitionsProvider)
		{
			var oldButtonDefinitions = oldButtonDefinitionsProvider.ButtonDefinitions;
			foreach (var disposable in oldButtonDefinitions.Select(definition => definition.Command).OfType<IDisposable>())
				disposable.Dispose();
			_buttonDefinitions.RemoveAll(oldButtonDefinitions);
		}
		AdditionalTooling = _toolingViewModelFactory.CreateToolingViewModel(value?.Value);
		if (AdditionalTooling is AnnotationButtonDefinitionsProvider newButtonDefinitionsProvider)
		{
			var newButtonDefinitions = newButtonDefinitionsProvider.ButtonDefinitions;
			_buttonDefinitions.AddRange(newButtonDefinitions);
		}
	}

	partial void OnAdditionalToolingChanged(object? value)
	{
		_additionalToolingChanged.OnNext(value);
	}

	private readonly Subject<ImageSetViewModel?> _selectedImageSetChanged = new();
	private readonly Subject<object?> _additionalToolingChanged = new();
	private readonly ToolingViewModelFactory _toolingViewModelFactory;
	private readonly AvaloniaList<AnnotationButtonDefinition> _buttonDefinitions = new();
	private readonly DataSetSelectionViewModel _dataSetSelection;
	private readonly IDisposable _selectedDataSetChangedSubscription;

	public void Dispose()
	{
		_selectedImageSetChanged.Dispose();
		_additionalToolingChanged.Dispose();
		foreach (var disposable in _buttonDefinitions.Select(definition => definition.Command).OfType<IDisposable>())
			disposable.Dispose();
		_selectedDataSetChangedSubscription.Dispose();
	}
}