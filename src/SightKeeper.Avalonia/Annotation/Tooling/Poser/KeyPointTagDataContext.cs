using System.Windows.Input;

namespace SightKeeper.Avalonia.Annotation.Tooling.Poser;

public interface KeyPointTagDataContext : TagDataContext
{
	ICommand DeleteKeyPointCommand { get; }
}