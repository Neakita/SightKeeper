using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DItem : PoserItem
{
	public Poser3DTag Tag
	{
		get => _tag;
		[MemberNotNull(nameof(_tag))] set
		{
			Guard.IsReferenceEqualTo(value.DataSet, DataSet);
			_tag?.RemoveItem(this);
			_tag = value;
			_tag.AddItem(this);
		}
	}

	public override IReadOnlyCollection<KeyPoint3D> KeyPoints => _keyPoints.Values;
	public Poser3DAsset Asset { get; }
	public DataSet DataSet => Asset.DataSet;

	public ImmutableList<double> NumericProperties
	{
		get => _numericProperties;
		[MemberNotNull(nameof(_numericProperties))] set
		{
			Guard.IsEqualTo(value.Count, Tag.NumericProperties.Count);
			_numericProperties = value;
		}
	}

	public ImmutableList<bool> BooleanProperties
	{
		get => _booleanProperties;
		[MemberNotNull(nameof(_booleanProperties))] set
		{
			Guard.IsEqualTo(value.Count, Tag.BooleanProperties.Count);
			_booleanProperties = value;
		}
	}

	public KeyPoint3D CreateKeyPoint(KeyPointTag3D tag, Vector2<double> position, bool isVisible)
	{
		KeyPoint3D keyPoint = new(position, this, tag, isVisible);
		_keyPoints.Add(tag, keyPoint);
		return keyPoint;
	}

	internal Poser3DItem(
		Poser3DTag tag,
		Bounding bounding,
		ImmutableList<double> numericProperties,
		ImmutableList<bool> booleanProperties,
		Poser3DAsset asset) : base(bounding)
	{
		Asset = asset;
		Tag = tag;
		NumericProperties = numericProperties;
		BooleanProperties = booleanProperties;
	}

	private ImmutableList<double> _numericProperties;
	private ImmutableList<bool> _booleanProperties;
	private Poser3DTag _tag;
	private Dictionary<KeyPointTag3D, KeyPoint3D> _keyPoints = new();
}