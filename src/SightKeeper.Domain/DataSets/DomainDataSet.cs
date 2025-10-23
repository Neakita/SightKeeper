using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets;

public sealed class DomainDataSet<TTag, TAsset> : DataSet<TTag, TAsset>, Decorator<DataSet<TTag, TAsset>> where TTag : Tag
{
	public string Name
	{
		get => Inner.Name;
		set => Inner.Name = value;
	}

	public string Description
	{
		get => Inner.Description;
		set => Inner.Description = value;
	}

	public TagsOwner<TTag> TagsLibrary { get; }
	public AssetsOwner<TAsset> AssetsLibrary { get; }
	public WeightsLibrary WeightsLibrary { get; }
	public DataSet<TTag, TAsset> Inner { get; }

	public DomainDataSet(DataSet<TTag, TAsset> inner, byte minimumTagsCount)
	{
		Inner = inner;
		TagsLibrary = new DomainTagsLibrary<TTag>(inner.TagsLibrary);
		AssetsLibrary = inner.AssetsLibrary;
		WeightsLibrary = new DomainWeightsLibrary(inner.WeightsLibrary, (TagsContainer<Tag>)TagsLibrary, minimumTagsCount);
	}
}