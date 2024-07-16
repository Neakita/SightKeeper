﻿using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
[MemoryPackUnion(0, typeof(Detector.SerializableDetectorWeights))]
public abstract partial class SerializableWeights
{
	public Id Id { get; }
	public DateTime CreationDate { get; }
	public ModelSize Size { get; }
	public WeightsMetrics Metrics { get; }

	[MemoryPackConstructor]
	protected SerializableWeights(Id id, DateTime creationDate, ModelSize size, WeightsMetrics metrics)
	{
		Id = id;
		CreationDate = creationDate;
		Size = size;
		Metrics = metrics;
	}

	protected SerializableWeights(Id id, Weights weights)
	{
		Id = id;
		CreationDate = weights.CreationDate;
		Size = weights.Size;
		Metrics = weights.Metrics;
	}
}