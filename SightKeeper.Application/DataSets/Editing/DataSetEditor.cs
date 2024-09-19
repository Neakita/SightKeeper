using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Editing;

public sealed class DataSetEditor : IDisposable
{
	public IObservable<DataSet> DataSetEdited => _dataSetEdited.AsObservable();

	public void Edit(DataSet dataSet, DataSetData data, IReadOnlyCollection<TagData> tagsData)
	{
		SetGeneralData(dataSet, data);
		UpdateTags(dataSet, tagsData);
	}

	public void Dispose()
	{
		_dataSetEdited.Dispose();
	}

	private readonly Subject<DataSet> _dataSetEdited = new();

	private static void SetGeneralData(DataSet dataSet, DataSetData data)
	{
		dataSet.Name = data.Name;
		dataSet.Description = data.Description;
		dataSet.Game = data.Game;
	}

	private static void UpdateTags(DataSet dataSet, IReadOnlyCollection<TagData> tagsData)
	{
		var orderedData = tagsData.Order(TagDataOperationPriorityComparer.Instance);
		foreach (var tagData in orderedData)
		{
			GuardDataSetEquality(dataSet, tagData);
			if (tagData is EditedTagData editedTagData)
				EditTag(editedTagData);
			else if (tagData is ExistingTagData existingTagData)
				RemoveTag(existingTagData);
			else if (tagData is NewTagData newTagData)
				CreateNewTag(dataSet.TagsLibrary, newTagData);
			else
				throw new ArgumentOutOfRangeException(nameof(tagData));
		}
	}

	private static void GuardDataSetEquality(DataSet dataSet, TagData tagData)
	{
		if (tagData is ExistingTagData existingTagData)
			Guard.IsReferenceEqualTo(existingTagData.Tag.DataSet, dataSet);
	}

	private static void RemoveTag(ExistingTagData existingTagData)
	{
		existingTagData.Tag.Delete();
	}

	private static void EditTag(EditedTagData editedTagData)
	{
		var tag = editedTagData.Tag;
		tag.Name = editedTagData.Name;
		tag.Color = editedTagData.Color;
		if (tag is PoserTag poserTag)
			EditKeyPoints(poserTag, ((EditedPoserTagData)editedTagData).Tags);
	}

	private static void EditKeyPoints(PoserTag poserTag, IReadOnlyCollection<TagData> tagsData)
	{
		var orderedData = tagsData.Order(TagDataOperationPriorityComparer.Instance);
		foreach (var tagData in orderedData)
		{
			GuardDataSetEquality(poserTag.DataSet, tagData);
			if (tagData is EditedTagData editedTagData)
				EditTag(editedTagData);
			else if (tagData is ExistingTagData existingTagData)
				RemoveTag(existingTagData);
			else if (tagData is NewTagData newTagData)
				CreateNewTag(poserTag, newTagData);
			else
				throw new ArgumentOutOfRangeException(nameof(tagData));
		}
	}

	private static void CreateNewTag(TagsHolder tagsHolder, NewTagData tagData)
	{
		var tag = tagsHolder.CreateTag(tagData.Name);
		tag.Color = tagData.Color;
		if (tag is PoserTag poserTag)
			CreateKeyPointTags(poserTag, (NewPoserTagData)tagData);
	}

	private static void CreateKeyPointTags(PoserTag tag, NewPoserTagData data)
	{
		foreach (var keyPointTagData in data.KeyPointTags)
		{
			var keyPointTag = tag.CreateKeyPoint(keyPointTagData.Name);
			keyPointTag.Color = keyPointTagData.Color;
		}
	}
}