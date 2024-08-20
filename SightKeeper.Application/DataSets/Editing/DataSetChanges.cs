using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Application.DataSets.Editing;

public interface DataSetChanges : GeneralDataSetInfo
{
    DetectorDataSet DataSet { get; }
    IReadOnlyCollection<TagInfo> NewTags { get; }
    IReadOnlyCollection<EditedTag> EditedTags { get; }
    IReadOnlyCollection<DeletedTag> DeletedTags { get; }
}