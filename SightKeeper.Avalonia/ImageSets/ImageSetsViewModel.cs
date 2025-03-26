using System;
using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Application;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ImageSets.Card;
using SightKeeper.Avalonia.ImageSets.Commands;
using SightKeeper.Domain.Images;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.ImageSets;

internal sealed class ImageSetsViewModel : ViewModel, ImageSetsDataContext, IDisposable
{
	public ReadOnlyObservableList<ImageSetCardViewModel> ImageSets { get; }
	IReadOnlyCollection<ImageSetCardDataContext> ImageSetsDataContext.ImageSets => ImageSets;
	public ICommand CreateImageSetCommand { get; }

	public ImageSetsViewModel(
		CreateImageSetCommandFactory createImageSetCommandFactory,
		ObservableRepository<ImageSet> imageSetsRepository,
		ImageSetEditor imageSetEditor,
		ImageSetCardViewModelFactory imageSetCardViewModelFactory)
	{
		CreateImageSetCommand = createImageSetCommandFactory.CreateCommand();
		ImageSets = imageSetsRepository.Items
			.Transform(imageSetCardViewModelFactory.CreateImageSetCardViewModel)
			.ToObservableList();
		_disposable = ImageSets.ToDictionary(imageSetViewModel => imageSetViewModel.ImageSet, out var imageSetCardViewModelsLookup);
		_imageSetCardViewModelsLookup = imageSetCardViewModelsLookup;
		imageSetEditor.Edited.Subscribe(OnImageSetEdited);
	}

	public void Dispose()
	{
		ImageSets.Dispose();
		_disposable.Dispose();
	}

	private readonly Dictionary<ImageSet, ImageSetCardViewModel> _imageSetCardViewModelsLookup;
	private readonly IDisposable _disposable;

	private void OnImageSetEdited(ImageSet set)
	{
		var viewModel = _imageSetCardViewModelsLookup[set];
		viewModel.NotifyPropertiesChanged();
	}
}