﻿using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableConstantScalingOptions))]
[MemoryPackUnion(1, typeof(SerializableIterativeScalingOptions))]
public abstract partial record SerializablePassiveScalingOptions;