using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public interface PoserDataSet : DataSet<PoserAsset>
{
	new TagsOwner<PoserTag> TagsLibrary { get; }

	TagsOwner<Tag> DataSet.TagsLibrary => TagsLibrary;
}