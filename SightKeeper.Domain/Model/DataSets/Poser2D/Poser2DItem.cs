using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DItem : PoserItem
{
	public Poser2DTag Tag { get; set; }
	public Bounding Bounding { get; set; }
	public IReadOnlyList<KeyPoint2D> KeyPoints { get; }

	public ImmutableList<double> Properties
	{
		get => _properties;
		[MemberNotNull(nameof(_properties))] set
		{
			Guard.IsEqualTo(value.Count, Tag.Properties.Count);
			_properties = value;
		}
	}

	internal Poser2DItem(Poser2DTag tag, Bounding bounding, IEnumerable<KeyPoint2D> keyPoints, ImmutableList<double> properties)
	{
		Tag = tag;
		Bounding = bounding;
		Properties = properties;
		KeyPoints = keyPoints.ToImmutableList();
	}

	private ImmutableList<double> _properties;
}