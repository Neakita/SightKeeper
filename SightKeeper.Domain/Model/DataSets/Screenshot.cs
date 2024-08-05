﻿namespace SightKeeper.Domain.Model.DataSets;

public abstract class Screenshot
{
	public DateTime CreationDate { get; } = DateTime.Now;
	public abstract Asset? Asset { get; }
	public abstract ScreenshotsLibrary Library { get; }
	public abstract DataSet DataSet { get; }

	protected internal abstract void DeleteFromLibrary();
}