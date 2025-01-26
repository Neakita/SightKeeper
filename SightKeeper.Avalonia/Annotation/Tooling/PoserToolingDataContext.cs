namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface PoserToolingDataContext
{
	TagSelectionToolingDataContext TagSelection { get; }
	TagSelectionToolingDataContext KeyPointTagSelection { get; }
}