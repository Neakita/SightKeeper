using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DTag : PoserTag, TagsFactory<Poser3DTag>
{
	static Poser3DTag TagsFactory<Poser3DTag>.Create(string name, TagsLibrary<Poser3DTag> library)
	{
		return new Poser3DTag(name, library);
	}

	public IReadOnlyList<KeyPointTag3D> KeyPoints => _keyPoints.AsReadOnly();
	public IReadOnlyList<NumericItemProperty> NumericProperties => _numericProperties.AsReadOnly();
	public IReadOnlyList<BooleanItemProperty> BooleanProperties => _booleanProperties.AsReadOnly();
	public override IReadOnlyCollection<Poser3DItem> Items => _items;
	public TagsLibrary<Poser3DTag> Library { get; }
	public override DataSet DataSet => Library.DataSet;

	public override KeyPointTag3D CreateKeyPoint(string name)
	{
		Guard.IsEmpty(Items);
		KeyPointTag3D tag = new(name, this);
		_keyPoints.Add(tag);
		return tag;
	}

	public void DeleteKeyPoint(KeyPointTag3D tag)
	{
		Guard.IsEmpty(Items);
		Guard.IsTrue(_keyPoints.Remove(tag));
	}

	public void CreateNumericProperty(string name, double minimumValue, double maximumValue)
	{
		Guard.IsEmpty(Items);
		NumericItemProperty property = new(this, name, minimumValue, maximumValue);
		_numericProperties.Add(property);
	}

	public void CreateBooleanProperty(string name)
	{
		Guard.IsEmpty(Items);
		BooleanItemProperty property = new(name);
		_booleanProperties.Add(property);
	}

	public void RemoveProperty(NumericItemProperty property)
	{
		Guard.IsEmpty(Items);
		Guard.IsTrue(_numericProperties.Remove(property));
	}

	public void RemoveProperty(BooleanItemProperty property)
	{
		Guard.IsEmpty(Items);
		Guard.IsTrue(_booleanProperties.Remove(property));
	}

	internal Poser3DTag(string name, TagsLibrary<Poser3DTag> library) : base(name, library)
	{
		Library = library;
		_keyPoints = new List<KeyPointTag3D>();
		_numericProperties = new List<NumericItemProperty>();
		_booleanProperties = new List<BooleanItemProperty>();
		_items = new HashSet<Poser3DItem>();
	}

	internal void AddItem(Poser3DItem item)
	{
		Guard.IsTrue(_items.Add(item));
	}

	internal void RemoveItem(Poser3DItem item)
	{
		Guard.IsTrue(_items.Remove(item));
	}

	protected override IEnumerable<Tag> Siblings => Library;

	private readonly List<KeyPointTag3D> _keyPoints;
	private readonly List<NumericItemProperty> _numericProperties;
	private readonly List<BooleanItemProperty> _booleanProperties;
	private readonly HashSet<Poser3DItem> _items;
}