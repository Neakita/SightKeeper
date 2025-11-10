using System.Windows.Input;
using SightKeeper.Avalonia.Annotation.Tooling.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling.Poser;

public interface KeyPointTagDataContext : TagDataContext
{
	ICommand DeleteKeyPointCommand { get; }
}