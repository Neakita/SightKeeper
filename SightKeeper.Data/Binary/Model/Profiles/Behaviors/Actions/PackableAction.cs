using MemoryPack;

namespace SightKeeper.Data.Binary.Model.Profiles.Behaviors.Actions;

/// <summary>
/// MemoryPackable version of <see cref="Domain.Model.Profiles.Actions.Action"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(0, typeof(PackablePressKeyAction))]
internal abstract partial class PackableAction
{
	public byte Tag { get; }

	protected PackableAction(byte tag)
	{
		Tag = tag;
	}
}