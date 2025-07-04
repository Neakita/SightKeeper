using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SightKeeper.Avalonia.Annotation.Tooling.Poser;

internal sealed class DesignPoserToolingDataContext : PoserToolingDataContext
{
	public static DesignPoserToolingDataContext WithoutTags { get; } = new();

	public static DesignPoserToolingDataContext WithPoserTags { get; } = new()
	{
		PoserTags =
		[
			new DesignTagDataContext("Ally"),
			new DesignTagDataContext("Enemy")
		]
	};

	public static DesignPoserToolingDataContext WithPoserAndKeyPointTags { get; } = new()
	{
		PoserTags =
		[
			new DesignTagDataContext("Ally"),
			new DesignTagDataContext("Enemy")
		],
		KeyPointTags = 
		[
			new DesignKeyPointTagDataContext("Head", true),
			new DesignKeyPointTagDataContext("Body", false)
		]
	};

	public IEnumerable<TagDataContext> PoserTags { get; init; } = Enumerable.Empty<TagDataContext>();
	public IReadOnlyCollection<KeyPointTagDataContext> KeyPointTags { get; init; } = ReadOnlyCollection<KeyPointTagDataContext>.Empty;

	public TagDataContext? SelectedPoserTag { get; set; }
	public TagDataContext? SelectedKeyPointTag { get; set; }
}