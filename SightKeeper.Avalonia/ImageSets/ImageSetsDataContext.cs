using System.Collections.Generic;
using System.Windows.Input;

namespace SightKeeper.Avalonia.ImageSets;

internal interface ImageSetsDataContext
{
	IReadOnlyCollection<ImageSetViewModel> ImageSets { get; }
	ICommand CreateImageSetCommand { get; }
	ICommand EditImageSetCommand { get; }
	ICommand DeleteImageSetCommand { get; }
}