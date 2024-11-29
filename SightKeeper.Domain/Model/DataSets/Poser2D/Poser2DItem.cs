using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DItem : PoserItem
{
	public Poser2DTag Tag
	{
		get => _tag;
		[MemberNotNull(nameof(_tag))] set
		{
			Guard.IsReferenceEqualTo(value.DataSet, DataSet);
			_tag?.RemoveItem(this);
			_tag = value;
			_tag.AddItem(this);
			_keyPoints.Clear();
		}
	}

	public override IReadOnlyCollection<KeyPoint2D> KeyPoints => _keyPoints.Values;

	public Poser2DAsset Asset { get; }
	public DataSet DataSet => Asset.DataSet;

	public ImmutableList<double> Properties
	{
		get => _properties;
		[MemberNotNull(nameof(_properties))] set
		{
			Guard.IsEqualTo(value.Count, Tag.Properties.Count);
			_properties = value;
		}
	}

	public KeyPoint2D CreateKeyPoint(KeyPointTag2D tag, Vector2<double> position)
	{
		KeyPoint2D keyPoint = new(position, this, tag);
		_keyPoints.Add(tag, keyPoint);
		tag.AddKeyPoint(keyPoint);
		return keyPoint;
	}

	public void DeleteKeyPoint(KeyPoint2D keyPoint)
	{
		bool isRemoved = _keyPoints.Remove(keyPoint.Tag, out var removedKeyPoint);
		Guard.IsTrue(isRemoved);
		Guard.IsNotNull(removedKeyPoint);
		Guard.IsReferenceEqualTo(keyPoint, removedKeyPoint);
	}

	internal Poser2DItem(Poser2DTag tag, Bounding bounding, ImmutableList<double> properties, Poser2DAsset asset) : base(bounding)
	{
		Asset = asset;
		Tag = tag;
		Properties = properties;
	}

	private readonly Dictionary<KeyPointTag2D, KeyPoint2D> _keyPoints = new();
	private ImmutableList<double> _properties;
	private Poser2DTag _tag;
}