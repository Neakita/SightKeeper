﻿using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableDetectorDataSet))]
public abstract partial record SerializableDataSet(string Name, string Description, Id? Game, ushort Resolution);

[MemoryPackable]
public partial record SerializableTag(Id Id, string Name, uint Color);

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableDetectorAsset))]
public abstract partial record SerializableAsset(Id ScreenshotId, AssetUsage Usage);

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableDetectorWeights))]
public abstract partial record SerializableWeights(Id Id, DateTime CreationDate, ModelSize Size, WeightsMetrics Metrics);