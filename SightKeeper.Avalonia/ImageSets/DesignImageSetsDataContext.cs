using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets;

internal class DesignImageSetsDataContext : ImageSetsDataContext
{
	public IReadOnlyCollection<ImageSetViewModel> ImageSets { get; } =
	[
		new(new ImageSet { Name = "PD2" }),
		new(new ImageSet { Name = "KF2" }),
		new(new ImageSet { Name = "Some dataset specific set" })
	];

	public ICommand CreateImageSetCommand { get; } = new RelayCommand(() => { });
	public ICommand EditImageSetCommand { get; } = new RelayCommand(() => { });
	public ICommand DeleteImageSetCommand { get; } = new RelayCommand(() => { });
}