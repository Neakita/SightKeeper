using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Application.DataSets;

public interface DataSetData
{
    string Name { get; }
    string Description { get; }
    TagsChanges TagsChanges { get; }
}