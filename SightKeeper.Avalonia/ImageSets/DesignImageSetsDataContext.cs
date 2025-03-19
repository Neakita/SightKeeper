using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.ImageSets.Card;

namespace SightKeeper.Avalonia.ImageSets;

internal class DesignImageSetsDataContext : ImageSetsDataContext
{
	public IReadOnlyCollection<ImageSetCardDataContext> ImageSets { get; } =
	[
		new DesignImageSetCardDataContext("PD2"),
		new DesignImageSetCardDataContext("KF2"),
		new DesignImageSetCardDataContext("Some dataset specific set")
	];

	public ICommand CreateImageSetCommand { get; } = new RelayCommand(() => { });
}