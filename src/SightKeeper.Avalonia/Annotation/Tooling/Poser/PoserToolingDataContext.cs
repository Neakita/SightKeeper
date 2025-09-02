using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Tooling.Poser;

public interface PoserToolingDataContext
{
	IEnumerable<TagDataContext> PoserTags { get; }
	TagDataContext? SelectedPoserTag { get; set; }
	IReadOnlyCollection<KeyPointTagDataContext> KeyPointTags { get; }
	TagDataContext? SelectedKeyPointTag { get; set; }
}