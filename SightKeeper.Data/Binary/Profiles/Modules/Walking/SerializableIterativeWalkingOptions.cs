using System.Numerics;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Walking;

[MemoryPackable]
public sealed partial record SerializableIterativeWalkingOptions(Vector<float> OffsetStep, Vector<byte> MaximumSteps) : SerializablePassiveWalkingOptions;