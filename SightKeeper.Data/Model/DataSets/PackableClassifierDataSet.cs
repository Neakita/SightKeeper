using MemoryPack;
using SightKeeper.Data.Model.DataSets.Tags;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Model.DataSets;

[MemoryPackable]
internal sealed partial class PackableClassifierDataSet : ClassifierDataSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	[MemoryPackIgnore] public TagsOwner<Tag> TagsLibrary { get; }
	[MemoryPackIgnore] public AssetsOwner<DomainClassifierAsset> AssetsLibrary => throw new NotImplementedException();
	[MemoryPackIgnore] public WeightsLibrary WeightsLibrary => throw new NotImplementedException();

	public PackableClassifierDataSet()
	{
		_packableTagsLibrary = new PackableTagsLibrary<PackableTag>();
		TagsLibrary = new DomainTagsLibrary<Tag>(_packableTagsLibrary);
		
		
	}

	[MemoryPackConstructor]
	public PackableClassifierDataSet(PackableTagsLibrary<PackableTag> packableTagsLibrary)
	{
		_packableTagsLibrary = packableTagsLibrary;
		TagsLibrary = new DomainTagsLibrary<Tag>(_packableTagsLibrary);
	}

	[MemoryPackInclude]
	private readonly PackableTagsLibrary<PackableTag> _packableTagsLibrary;
}