using MemoryPack;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

namespace SightKeeper.Data.Binary.Profiles.Modules;

[MemoryPackable]
internal sealed partial class SerializableClassifierModule : SerializableModule
{
	public SerializableClassifierModule(SerializableTriggerBehaviour behaviour) : base(behaviour)
	{
	}
}