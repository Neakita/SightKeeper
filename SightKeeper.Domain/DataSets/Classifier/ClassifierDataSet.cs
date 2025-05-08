using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Classifier;

public sealed class ClassifierDataSet : DataSet
{
	public override TagsLibrary<Tag> TagsLibrary { get; }
	public override AssetsLibrary<ClassifierAsset> AssetsLibrary { get; }
	public override PlainWeightsLibrary WeightsLibrary { get; }

	public ClassifierDataSet()
	{
		TagsLibrary = new TagsLibrary<Tag>(PlainTagsFactory.Instance);
		AssetsLibrary = new AssetsLibrary<ClassifierAsset>(new ClassifierAssetsFactory(TagsLibrary));
		WeightsLibrary = new PlainWeightsLibrary(2, TagsLibrary);
	}
}