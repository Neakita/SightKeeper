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
	public IReadOnlyCollection<DataSetViewModel> DataSets { get; }
	[ObservableProperty] public partial DataSetViewModel? SelectedDataSet { get; set; }
	[ObservableProperty] public partial ImageSetViewModel? SelectedImageSet { get; set; }
	public IObservable<ImageSetViewModel?> SelectedImageSetChanged => _selectedImageSetChanged.AsObservable();
	public IObservable<DataSetViewModel?> SelectedDataSetChanged => _selectedDataSetChanged.AsObservable();
	public IReadOnlyCollection<AnnotationButtonDefinition> ButtonDefinitions => _buttonDefinitions;
	[ObservableProperty] public partial object? AdditionalTooling { get; private set; }
	public IObservable<object?> AdditionalToolingChanged => _additionalToolingChanged.AsObservable();

	IReadOnlyCollection<ImageSetDataContext> SideBarDataContext.ImageSets => ImageSets;

	ImageSetDataContext? SideBarDataContext.SelectedImageSet
	{
		get => SelectedImageSet;
		set => SelectedImageSet = (ImageSetViewModel?)value;
	}

	IReadOnlyCollection<DataSetDataContext> SideBarDataContext.DataSets => DataSets;

	DataSetDataContext? SideBarDataContext.SelectedDataSet
	{
		get => SelectedDataSet;
		set => SelectedDataSet = (DataSetViewModel?)value;
	}

	public SideBarViewModel(
		DataSetViewModelsObservableRepository dataSetsRepository,
		ImageSetViewModelsObservableRepository imageSets,
		ToolingViewModelFactory toolingViewModelFactory,
		IEnumerable<AnnotationButtonDefinitionFactory> buttonDefinitionFactories)
	{
		_toolingViewModelFactory = toolingViewModelFactory;
		DataSets = dataSetsRepository.Items;
		ImageSets = imageSets.Items;
		_buttonDefinitions.AddRange(buttonDefinitionFactories.Select(factory => factory.CreateButtonDefinition()));
	}

	partial void OnSelectedImageSetChanged(ImageSetViewModel? value)
	{
		_selectedImageSetChanged.OnNext(value);
	}

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		_selectedDataSetChanged.OnNext(value);
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
	private readonly Subject<DataSetViewModel?> _selectedDataSetChanged = new();
	private readonly Subject<object?> _additionalToolingChanged = new();
	private readonly ToolingViewModelFactory _toolingViewModelFactory;
	private readonly AvaloniaList<AnnotationButtonDefinition> _buttonDefinitions = new();

	public void Dispose()
	{
		_selectedImageSetChanged.Dispose();
		_selectedDataSetChanged.Dispose();
		_additionalToolingChanged.Dispose();
		foreach (var disposable in _buttonDefinitions.Select(definition => definition.Command).OfType<IDisposable>())
			disposable.Dispose();
	}
}