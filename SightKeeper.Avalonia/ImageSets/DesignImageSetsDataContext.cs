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
		new DesignImageSetCardDataContext("KF2 Sample 1", "kfSample1.jpg"),
		new DesignImageSetCardDataContext("KF2 Sample 2", "kfSample2.jpg"),
		new DesignImageSetCardDataContext("KF2 Sample 3", "kfSample3.jpg"),
		new DesignImageSetCardDataContext("KF2 Sample 4", "kfSample4.jpg"),
		new DesignImageSetCardDataContext("KF2 Sample 5", "kfSample5.jpg"),
		new DesignImageSetCardDataContext("KF2 Sample 6", "kfSample6.jpg"),
		new DesignImageSetCardDataContext("KF2 Sample 7", "kfSample7.jpg"),
		new DesignImageSetCardDataContext("KF2 Sample 8", "kfSample8.jpg"),
		
	];

	public ICommand CreateImageSetCommand { get; } = new RelayCommand(() => { });
}