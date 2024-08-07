using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
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

	public Bounding Bounding { get; set; }
	public IReadOnlyList<KeyPoint3D> KeyPoints { get; }
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

	internal Poser3DItem(
		Poser3DTag tag,
		Bounding bounding,
		IEnumerable<KeyPoint3D> keyPoints,
		ImmutableList<double> numericProperties,
		ImmutableList<bool> booleanProperties,
		Poser3DAsset asset)
	{
		Asset = asset;
		Tag = tag;
		Bounding = bounding;
		KeyPoints = keyPoints.ToImmutableList();
		NumericProperties = numericProperties;
		BooleanProperties = booleanProperties;
	}

	private ImmutableList<double> _numericProperties;
	private ImmutableList<bool> _booleanProperties;
	private Poser3DTag _tag;
}