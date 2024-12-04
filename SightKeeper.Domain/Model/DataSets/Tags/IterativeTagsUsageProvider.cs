using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Domain.Model.DataSets.Tags;

internal abstract class IterativeTagsUsageProvider<TAsset> : TagsUsageProvider where TAsset : Asset
{
	public IEnumerable<TAsset> AssetsSource { get; set; } = Enumerable.Empty<TAsset>();

	public override bool IsInUse(Tag tag)
	{
		Guard.IsNotNull(AssetsSource);
		return AssetsSource.Any(asset => IsInUse(asset, tag));
	}

	protected abstract bool IsInUse(TAsset asset, Tag tag);

}