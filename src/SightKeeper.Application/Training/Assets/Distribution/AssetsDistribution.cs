using System.Collections.ObjectModel;

namespace SightKeeper.Application.Training.Assets.Distribution;

internal sealed class AssetsDistribution<TAsset>
{
	public static AssetsDistribution<TAsset> Empty { get; } = new()
	{
		TrainAssets = ReadOnlyCollection<TAsset>.Empty,
		ValidationAssets = ReadOnlyCollection<TAsset>.Empty,
		TestAssets = ReadOnlyCollection<TAsset>.Empty
	};

	public required IReadOnlyList<TAsset> TrainAssets { get; init; }
	public required IReadOnlyList<TAsset> ValidationAssets { get; init; }
	public required IReadOnlyList<TAsset> TestAssets { get; init; }
}