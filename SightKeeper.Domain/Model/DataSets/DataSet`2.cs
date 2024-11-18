using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class DataSet<TTag, TAsset> : DataSet<TAsset>
	where TTag : Tag, TagsFactory<TTag>, MinimumTagsCount
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public override TagsLibrary<TTag> TagsLibrary { get; }
	public override WeightsLibrary<TTag> WeightsLibrary { get; }

	protected DataSet()
	{
		TagsLibrary = new TagsLibrary<TTag>(this);
		WeightsLibrary = new WeightsLibrary<TTag>(this);
	}
}