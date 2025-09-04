using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets;

public sealed class DomainDataSet<TAsset> : DataSet<TAsset>, Decorator<DataSet<TAsset>>
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

	public TagsOwner<Tag> TagsLibrary { get; }
	public AssetsOwner<TAsset> AssetsLibrary { get; }
	public WeightsLibrary WeightsLibrary { get; }
	public DataSet<TAsset> Inner { get; }

	public DomainDataSet(DataSet<TAsset> inner, int minimumTagsCount)
	{
		Inner = inner;
		TagsLibrary = new DomainTagsLibrary<Tag>(inner.TagsLibrary);
		AssetsLibrary = inner.AssetsLibrary;
		WeightsLibrary = new DomainWeightsLibrary(inner.WeightsLibrary, TagsLibrary, minimumTagsCount);
	}
}