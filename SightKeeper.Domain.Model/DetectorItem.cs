﻿namespace SightKeeper.Domain.Model;

public sealed class DetectorItem : Entity
{
	public ItemClass ItemClass { get; set; }
	public Bounding Bounding { get; set; }
	
	internal DetectorItem(ItemClass itemClass, Bounding bounding)
	{
		ItemClass = itemClass;
		Bounding = bounding;
	}
	
	private DetectorItem()
	{
		ItemClass = null!;
	}
}