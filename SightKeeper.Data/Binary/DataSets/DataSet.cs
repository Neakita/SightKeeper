﻿using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.DataSets.Detector;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
[MemoryPackUnion(0, typeof(DetectorDataSet))]
internal abstract partial class DataSet
{
	public string Name { get; }
	public string Description { get; }
	public ushort? GameId { get; }
	public ushort Resolution { get; }
	public ushort? MaxScreenshots { get; }
	public ImmutableArray<Screenshot> Screenshots { get; }

	[MemoryPackConstructor]
	protected DataSet(
		string name,
		string description,
		ushort? gameId,
		ushort resolution,
		ushort? maxScreenshots,
		ImmutableArray<Screenshot> screenshots)
	{
		Name = name;
		Description = description;
		GameId = gameId;
		Resolution = resolution;
		MaxScreenshots = maxScreenshots;
		Screenshots = screenshots;
	}

	protected DataSet(Domain.Model.DataSets.DataSet dataSet, ushort? gameId, ImmutableArray<Screenshot> screenshots)
	{
		throw new NotImplementedException();
		// Name = dataSet.Name;
		// Description = dataSet.Description;
		// GameId = gameId;
		// MaxScreenshots = dataSet.Screenshots.MaxQuantity;
		// Screenshots = screenshots;
	}
}