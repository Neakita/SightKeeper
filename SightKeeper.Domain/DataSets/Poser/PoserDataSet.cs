using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Poser;

public abstract class PoserDataSet : DataSet
{
	public sealed override DomainTagsLibrary<PoserTag> TagsLibrary { get; }
	public abstract override AssetsOwner<PoserAsset> AssetsLibrary { get; }
	public sealed override DomainWeightsLibrary WeightsLibrary { get; }

	protected PoserDataSet()
	{
		TagsLibrary = new DomainTagsLibrary<PoserTag>(PoserTagsFactory.Instance)
		{
			DataSet = this
		};
		WeightsLibrary = new DomainWeightsLibrary(TagsLibrary);
	}
}