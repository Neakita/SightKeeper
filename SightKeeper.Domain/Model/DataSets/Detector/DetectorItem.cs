﻿using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorItem : AssetItem
{
	public DetectorTag Tag
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
	public DetectorAsset Asset { get; }
	public DataSet DataSet => Asset.DataSet;
	
	internal DetectorItem(DetectorTag tag, Bounding bounding, DetectorAsset asset)
	{
		Asset = asset;
		Tag = tag;
		Bounding = bounding;
	}

	private DetectorTag _tag;
}