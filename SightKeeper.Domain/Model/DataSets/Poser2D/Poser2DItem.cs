using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
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
		}
	}

	public Bounding Bounding { get; set; }

	public ImmutableList<Vector2<double>> KeyPoints
	{
		get => _keyPoints;
		[MemberNotNull(nameof(_keyPoints))] set
		{
			Guard.IsEqualTo(value.Count, Tag.KeyPoints.Count);
			_keyPoints = value;
		}
	}

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

	internal Poser2DItem(Poser2DTag tag, Bounding bounding, ImmutableList<Vector2<double>> keyPoints, ImmutableList<double> properties, Poser2DAsset asset)
	{
		Asset = asset;
		Tag = tag;
		Bounding = bounding;
		Properties = properties;
		KeyPoints = keyPoints;
	}

	private ImmutableList<double> _properties;
	private Poser2DTag _tag;
	private ImmutableList<Vector2<double>> _keyPoints;
}