using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Behaviors;

namespace SightKeeper.Data.Binary.Model.Profiles.Behaviors;

/// <summary>
/// MemoryPackable version of <see cref="AimBehavior.TagOptions"/>
/// </summary>
[MemoryPackable]
public sealed partial class PackableAimBehaviorTagOptions
{
	public byte Priority { get; }
	public float VerticalOffset { get; }

	public PackableAimBehaviorTagOptions(byte priority, float verticalOffset)
	{
		Priority = priority;
		VerticalOffset = verticalOffset;
	}
}