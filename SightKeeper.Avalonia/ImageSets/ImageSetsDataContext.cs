using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Avalonia.ImageSets.Card;

namespace SightKeeper.Avalonia.ImageSets;

internal interface ImageSetsDataContext
{
	IReadOnlyCollection<ImageSetCardDataContext> ImageSets { get; }
	ICommand CreateImageSetCommand { get; }
}