﻿namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DDataSet : DataSet<Poser3DTag, KeyPointTag3D, Poser3DAsset>
{
	public Poser3DDataSet(string name, ushort resolution) : base(name, resolution)
	{
	}
}