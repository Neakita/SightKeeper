using System.Windows.Input;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public interface KeyPointDrawerDataContext
{
	ICommand CreateKeyPointCommand { get; }
	KeyPointViewModel? ExistingKeyPoint { get; }
}