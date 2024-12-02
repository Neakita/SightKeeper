using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class DataSet<TTag, TKeyPointTag, TAsset> : DataSet<TAsset>
	where TTag : PoserTag, TagsFactory<TTag>
	where TKeyPointTag : KeyPointTag<TTag>
	where TAsset : Asset
{
	public override TagsLibrary<TTag> TagsLibrary { get; }
	public override WeightsLibrary<TTag, TKeyPointTag> WeightsLibrary { get; }

	protected DataSet()
	{
		TagsLibrary = new TagsLibrary<TTag>(this);
		WeightsLibrary = new WeightsLibrary<TTag, TKeyPointTag>(this);
	}
}