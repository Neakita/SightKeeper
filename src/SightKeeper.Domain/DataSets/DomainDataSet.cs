using SightKeeper.Domain.DataSets.Artifacts;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets;

public sealed class DomainDataSet<TTag, TAsset>(DataSet<TTag, TAsset> inner)
	: DataSet<TTag, TAsset>, Decorator<DataSet<TTag, TAsset>>
	where TTag : Tag
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

	public TagsOwner<TTag> TagsLibrary { get; } = new DomainTagsLibrary<TTag>(inner.TagsLibrary);
	public AssetsOwner<TAsset> AssetsLibrary => Inner.AssetsLibrary;
	public ArtifactsLibrary ArtifactsLibrary => Inner.ArtifactsLibrary;
	public DataSet<TTag, TAsset> Inner => inner;
}