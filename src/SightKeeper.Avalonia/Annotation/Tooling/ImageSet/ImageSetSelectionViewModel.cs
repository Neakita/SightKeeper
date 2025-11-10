using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SightKeeper.Avalonia.Annotation.Tooling.ImageSet;

internal sealed partial class ImageSetSelectionViewModel : ViewModel, ImageSetSelectionDataContext, ImageSetSelection, IDisposable
{
	public IReadOnlyCollection<ImageSetViewModel> ImageSets { get; }
	[ObservableProperty] public partial ImageSetViewModel? SelectedImageSet { get; set; }

	IReadOnlyCollection<ImageSetDataContext> ImageSetSelectionDataContext.ImageSets => ImageSets;

	ImageSetDataContext? ImageSetSelectionDataContext.SelectedImageSet
	{
		get => SelectedImageSet;
		set => SelectedImageSet = (ImageSetViewModel?)value;
	}

	IObservable<Domain.Images.ImageSet?> ImageSetSelection.SelectedImageSetChanged =>
		_selectedImageSetChanged.Select(viewModel => viewModel?.Value);

	Domain.Images.ImageSet? ImageSetSelection.SelectedImageSet => SelectedImageSet?.Value;

	public ImageSetSelectionViewModel(ImageSetViewModelsObservableListRepository imageSets)
	{
		ImageSets = imageSets.Items;
	}

	public void Dispose()
	{
		_selectedImageSetChanged.Dispose();
	}

	private readonly Subject<ImageSetViewModel?> _selectedImageSetChanged = new();

	partial void OnSelectedImageSetChanged(ImageSetViewModel? value)
	{
		_selectedImageSetChanged.OnNext(value);
	}
}