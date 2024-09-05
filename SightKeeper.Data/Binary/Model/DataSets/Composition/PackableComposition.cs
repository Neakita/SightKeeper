using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Model.DataSets.Composition;

/// <summary>
/// MemoryPackable version of <see cref="Composition"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(0, typeof(PackableTransparentComposition))]
internal abstract partial class PackableComposition
{
	public TimeSpan MaximumScreenshotsDelay { get; }

	protected PackableComposition(TimeSpan maximumScreenshotsDelay)
	{
		MaximumScreenshotsDelay = maximumScreenshotsDelay;
	}
}