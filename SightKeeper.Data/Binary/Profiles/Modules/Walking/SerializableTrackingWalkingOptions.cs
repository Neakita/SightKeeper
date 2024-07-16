using MemoryPack;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Profiles.Modules.Walking;

[MemoryPackable]
public sealed partial record SerializableTrackingWalkingOptions(Vector2<float> MaximumOffset) : SerializableActiveWalkingOptions;