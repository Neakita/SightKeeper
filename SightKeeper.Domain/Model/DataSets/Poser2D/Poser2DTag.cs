using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DTag : PoserTag, TagsFactory<Poser2DTag>
{
	static Poser2DTag TagsFactory<Poser2DTag>.Create(string name, TagsLibrary<Poser2DTag> library)
	{
		return new Poser2DTag(name, library);
	}

	public override IReadOnlyList<KeyPointTag2D> KeyPoints => _keyPoints.AsReadOnly();
	public IReadOnlyList<NumericItemProperty> Properties => _properties.AsReadOnly();
	public override IReadOnlyCollection<Poser2DItem> Items => _items;
	public TagsLibrary<Poser2DTag> Library { get; }
	public override DataSet DataSet => Library.DataSet;

	public override KeyPointTag2D CreateKeyPoint(string name)
	{
		Guard.IsEmpty(Items);
		KeyPointTag2D tag = new(name, this);
		_keyPoints.Add(tag);
		return tag;
	}

	public void DeleteKeyPoint(KeyPointTag2D tag)
	{
		Guard.IsEmpty(Items);
		Guard.IsTrue(_keyPoints.Remove(tag));
	}

	public NumericItemProperty CreateProperty(string name, double minimumValue, double maximumValue)
	{
		NumericItemProperty property = new(this, name, minimumValue, maximumValue);
		_properties.Add(property);
		return property;
	}

	public void RemoveProperty(NumericItemProperty property)
	{
		_properties.Remove(property);
	}

	public override void Delete()
	{
		Library.DeleteTag(this);
	}

	internal Poser2DTag(string name, TagsLibrary<Poser2DTag> library) : base(name, library.Tags)
	{
		Library = library;
		_keyPoints = new List<KeyPointTag2D>();
		_properties = new List<NumericItemProperty>();
		_items = new HashSet<Poser2DItem>();
	}

	internal void AddItem(Poser2DItem item)
	{
		Guard.IsTrue(_items.Add(item));
	}

	internal void RemoveItem(Poser2DItem item)
	{
		Guard.IsTrue(_items.Remove(item));
	}

	protected override IEnumerable<Tag> Siblings => Library.Tags;

	private readonly List<KeyPointTag2D> _keyPoints;
	private readonly List<NumericItemProperty> _properties;
	private readonly HashSet<Poser2DItem> _items;
}