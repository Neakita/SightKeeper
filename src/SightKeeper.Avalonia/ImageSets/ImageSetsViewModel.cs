using System;
using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Application.Misc;
using SightKeeper.Avalonia.ImageSets.Capturing;
using SightKeeper.Avalonia.ImageSets.Card;
using SightKeeper.Domain.Images;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.ImageSets;

public sealed class ImageSetsViewModel : ViewModel, ImageSetsDataContext, IDisposable
{
	public IReadOnlyCollection<ImageSetCardDataContext> ImageSets { get; }
	public ICommand CreateImageSetCommand { get; }

	public CapturingSettingsDataContext CapturingSettings { get; }

	public ImageSetsViewModel(
		ICommand createImageSetCommand,
		ObservableListRepository<ImageSet> imageSetsListRepository,
		ImageSetCardDataContextFactory imageSetCardDataContextFactory,
		CapturingSettingsDataContext capturingSettings)
	{
		CreateImageSetCommand = createImageSetCommand;
		var imageSets = imageSetsListRepository.Items
			.Transform(imageSetCardDataContextFactory.CreateImageSetCardDataContext)
			.DisposeMany()
			.ToObservableList();
		_disposable = imageSets;
		ImageSets = imageSets.ToReadOnlyNotifyingList();
		CapturingSettings = capturingSettings;
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly IDisposable _disposable;
}