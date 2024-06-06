﻿namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorItem
{
	public Tag Tag { get; set; }
	public Bounding Bounding { get; set; }
	
	internal DetectorItem(Tag tag, Bounding bounding)
	{
		Tag = tag;
		Bounding = bounding;
	}
}