﻿using System.Collections.Immutable;
using MemoryPack;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
public sealed partial class RawAppData
{
	public ImmutableArray<DataSets.Detector.SerializableDetectorDataSet> DetectorDataSets { get; }
	public ImmutableArray<SerializableGame> Games { get; }
	public ImmutableArray<Profiles.SerializableProfile> Profiles { get; }

	public RawAppData(
		ImmutableArray<DataSets.Detector.SerializableDetectorDataSet> detectorDataSets,
		ImmutableArray<SerializableGame> games,
		ImmutableArray<Profiles.SerializableProfile> profiles)
	{
		DetectorDataSets = detectorDataSets;
		Games = games;
		Profiles = profiles;
	}
}