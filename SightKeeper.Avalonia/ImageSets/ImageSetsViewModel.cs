using System;
using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Application;
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
		ImageSetCardViewModelFactory imageSetCardViewModelFactory,
		CapturingSettingsViewModel capturingSettings)
	{
		CreateImageSetCommand = createImageSetCommand;
		ImageSets = imageSetsListRepository.Items
			.Transform(imageSetCardViewModelFactory.CreateImageSetCardViewModel)
			.DisposeMany()
			.ToObservableList();
		CapturingSettings = capturingSettings;
	}

	public void Dispose()
	{
		ImageSets.Dispose();
	}
}