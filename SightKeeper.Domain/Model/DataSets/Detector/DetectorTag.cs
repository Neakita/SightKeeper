using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorTag : ItemTag, MinimumTagsCount, TagsFactory<DetectorTag>
{
	public static byte MinimumCount => 1;

	static DetectorTag TagsFactory<DetectorTag>.Create(string name, TagsLibrary<DetectorTag> library)
	{
		return new DetectorTag(name, library);
	}

	public override IReadOnlyCollection<DetectorItem> Items => _items;
	public TagsLibrary<DetectorTag> Library { get; }
	public override DataSet DataSet => Library.DataSet;

	public override void Delete()
	{
		Library.DeleteTag(this);
	}

	internal DetectorTag(string name, TagsLibrary<DetectorTag> library) : base(name, library.Tags)
	{
		Library = library;
		_items = new HashSet<DetectorItem>();
	}

	internal void AddItem(DetectorItem item)
	{
		Guard.IsTrue(_items.Add(item));
	}

	internal void RemoveItem(DetectorItem item)
	{
		Guard.IsTrue(_items.Remove(item));
	}

	protected override IEnumerable<Tag> Siblings => Library.Tags;
	private readonly HashSet<DetectorItem> _items;
}