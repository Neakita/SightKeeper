using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Model.DataSets.Compositions;

/// <summary>
/// MemoryPackable version of <see cref="TransparentComposition"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableTransparentComposition : PackableComposition
{
	public ImmutableArray<float> Opacities { get; }

	public PackableTransparentComposition(TimeSpan maximumScreenshotsDelay, ImmutableArray<float> opacities) : base(maximumScreenshotsDelay)
	{
		Opacities = opacities;
	}
}