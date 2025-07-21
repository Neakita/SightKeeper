using System;
using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Application;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ImageSets.Capturing;
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

	public CapturingSettingsDataContext CapturingSettings { get; }

	public ImageSetsViewModel(
		CreateImageSetCommand createImageSetCommand,
		ObservableListRepository<ImageSet> imageSetsListRepository,
		ImageSetEditor imageSetEditor,
		ImageSetCardViewModelFactory imageSetCardViewModelFactory,
		CapturingSettingsViewModel capturingSettings)
	{
		CreateImageSetCommand = createImageSetCommand;
		ImageSets = imageSetsListRepository.Items
			.Transform(imageSetCardViewModelFactory.CreateImageSetCardViewModel)
			.DisposeMany()
			.ToObservableList();
		_disposable = ImageSets.ToDictionary(imageSetViewModel => imageSetViewModel.ImageSet, out var imageSetCardViewModelsLookup);
		_imageSetCardViewModelsLookup = imageSetCardViewModelsLookup;
		imageSetEditor.Edited.Subscribe(OnImageSetEdited);
		CapturingSettings = capturingSettings;
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