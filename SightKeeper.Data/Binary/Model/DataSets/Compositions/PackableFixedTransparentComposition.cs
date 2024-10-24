using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Model.DataSets.Compositions;

/// <summary>
/// MemoryPackable version of <see cref="FixedTransparentComposition"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableFixedTransparentComposition : PackableComposition
{
	public ImmutableArray<float> Opacities { get; }

	public PackableFixedTransparentComposition(TimeSpan maximumScreenshotsDelay, ImmutableArray<float> opacities) : base(maximumScreenshotsDelay)
	{
		Opacities = opacities;
	}
}