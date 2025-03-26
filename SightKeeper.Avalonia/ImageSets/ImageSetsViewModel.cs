using System;
using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Application;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Avalonia.Annotation.Images;
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
		EditImageSetCommandFactory editImageSetCommandFactory,
		DeleteImageSetCommandFactory deleteImageSetCommandFactory,
		ObservableRepository<ImageSet> imageSetsRepository,
		ImageSetEditor imageSetEditor,
		ImageLoader imageLoader)
	{
		_imageLoader = imageLoader;
		CreateImageSetCommand = createImageSetCommandFactory.CreateCommand();
		_editImageSetCommand = editImageSetCommandFactory.CreateCommand();
		_deleteImageSetCommand = deleteImageSetCommandFactory.CreateCommand();
		ImageSets = imageSetsRepository.Items.Transform(CreateImageSetViewModel).ToObservableList();
		_disposable = ImageSets.ToDictionary(imageSetViewModel => imageSetViewModel.ImageSet, out var imageSetCardViewModelsLookup);
		_imageSetCardViewModelsLookup = imageSetCardViewModelsLookup;
		imageSetEditor.Edited.Subscribe(OnImageSetEdited);
	}

	public void Dispose()
	{
		ImageSets.Dispose();
		_disposable.Dispose();
	}

	private readonly ICommand _editImageSetCommand;
	private readonly ICommand _deleteImageSetCommand;
	private readonly Dictionary<ImageSet, ImageSetCardViewModel> _imageSetCardViewModelsLookup;
	private readonly ImageLoader _imageLoader;
	private readonly IDisposable _disposable;

	private ImageSetCardViewModel CreateImageSetViewModel(ImageSet imageSet)
	{
		ParametrizedCommandAdapter editCommand = new(_editImageSetCommand, imageSet);
		ParametrizedCommandAdapter deleteCommand = new(_deleteImageSetCommand, imageSet);
		return new ImageSetCardViewModel(imageSet, editCommand, deleteCommand, _imageLoader);
	}

	private void OnImageSetEdited(ImageSet set)
	{
		var viewModel = _imageSetCardViewModelsLookup[set];
		viewModel.NotifyPropertiesChanged();
	}
}