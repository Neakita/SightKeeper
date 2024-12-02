using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Actions;

namespace SightKeeper.Data.Binary.Model.Profiles.Behaviors.Actions;

/// <summary>
/// MemoryPackable version of <see cref="PressKeyAction"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePressKeyAction : PackableAction
{
	public byte Type { get; }
	public ushort KeyCode { get; }

	public PackablePressKeyAction(byte tag, byte type, ushort keyCode) : base(tag)
	{
		Type = type;
		KeyCode = keyCode;
	}
}