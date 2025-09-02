using System.Windows.Input;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public interface KeyPointDrawerDataContext
{
	ICommand CreateKeyPointCommand { get; }
	KeyPointViewModel? ExistingKeyPoint { get; }
}