using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Profiles.Behaviors;

namespace SightKeeper.Data.Binary.Model.Profiles.Behaviors;

/// <summary>
/// MemoryPackable version of <see cref="AimAssistBehavior.TagOptions"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableAimAssistBehaviorTagOptions
{
	public byte Priority { get; } 
	public Vector2<float> TargetAreaScale { get; }
	public float VerticalOffset { get; }

	public PackableAimAssistBehaviorTagOptions(byte priority, Vector2<float> targetAreaScale, float verticalOffset)
	{
		Priority = priority;
		TargetAreaScale = targetAreaScale;
		VerticalOffset = verticalOffset;
	}
}