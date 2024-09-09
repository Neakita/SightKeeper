using MemoryPack;
using SightKeeper.Data.Binary.Model.Profiles.PassiveScalingOptions;
using SightKeeper.Data.Binary.Model.Profiles.PassiveWalkingOptions;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Model.Profiles.Modules;

/// <summary>
/// MemoryPackable version of <see cref="Module"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(0, typeof(PackableClassifierModule))]
[MemoryPackUnion(1, typeof(PackableDetectorModule))]
[MemoryPackUnion(2, typeof(PackablePoser2DModule))]
[MemoryPackUnion(3, typeof(PackablePoser3DModule))]
internal abstract partial class PackableModule
{
	public ushort WeightsId { get; }
	public PackablePassiveScalingOptions? PassiveScalingOptions { get; }
	public PackablePassiveWalkingOptions? PassiveWalkingOptions { get; }

	public PackableModule(
		ushort weightsId,
		PackablePassiveScalingOptions? passiveScalingOptions,
		PackablePassiveWalkingOptions? passiveWalkingOptions)
	{
		WeightsId = weightsId;
		PassiveScalingOptions = passiveScalingOptions;
		PassiveWalkingOptions = passiveWalkingOptions;
	}
}