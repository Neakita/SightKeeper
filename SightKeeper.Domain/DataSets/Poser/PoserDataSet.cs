using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Poser;

public abstract class PoserDataSet : DataSet
{
	public sealed override TagsLibrary<PoserTag> TagsLibrary { get; }
	public sealed override WeightsLibrary WeightsLibrary { get; }

	protected PoserDataSet()
	{
		TagsLibrary = new TagsLibrary<PoserTag>(PoserTagsFactory.Instance);
		WeightsLibrary = new WeightsLibrary(TagsLibrary);
	}
}