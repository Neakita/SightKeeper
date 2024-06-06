using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Application.DataSets.Editing;

public sealed class DataSetChangesDTO : DataSetChanges
{
    public DetectorDataSet DataSet { get; }
    public string Name { get; }
    public string Description { get; }
    public Game? Game { get; }
    public IReadOnlyCollection<TagInfo> Tags { get; }
    public IReadOnlyCollection<TagInfo> NewTags { get; }
    public IReadOnlyCollection<EditedTag> EditedTags { get; }
    public IReadOnlyCollection<DeletedTag> DeletedTags { get; }

    public DataSetChangesDTO(DetectorDataSet dataSet, DataSetChanges changes)
    {
        DataSet = dataSet;
        Name = changes.Name;
        Description = changes.Description;
        Tags = changes.Tags.ToList();
        Game = changes.Game;
        NewTags = changes.NewTags.ToList();
        EditedTags = changes.EditedTags.ToList();
        DeletedTags = changes.DeletedTags.ToList();
    }
    
    public DataSetChangesDTO(
        DetectorDataSet dataSet,
        string name, string description, Game? game,
        IEnumerable<TagInfo> newTags,
        IEnumerable<EditedTag> editedTags,
        IEnumerable<DeletedTag> deletedTags)
    {
        DataSet = dataSet;
        Name = name;
        Description = description;
        NewTags = newTags.ToList();
        EditedTags = editedTags.ToList();
        DeletedTags = deletedTags.ToList();
        Tags = DataSet.Tags
            .Except(DeletedTags.Select(deletedTag => deletedTag.Tag))
            .Select(existingTag => new TagInfo(existingTag))
            .Concat(NewTags)
            .ToList();
        Game = game;
    }
}