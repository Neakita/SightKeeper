using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Editing;

public abstract class DataSetEditor : IDisposable
{
	public IObservable<DataSet> DataSetEdited => _dataSetEdited.AsObservable();

	public virtual void Edit(DataSet dataSet, DataSetData data, IReadOnlyCollection<TagData> tagsData)
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
	}

	private static void UpdateTags(DataSet dataSet, IReadOnlyCollection<TagData> tagsData)
	{
		var orderedData = tagsData.Order(TagDataOperationPriorityComparer.Instance);
		foreach (var tagData in orderedData)
		{
			if (tagData is EditedTagData editedTagData)
				EditTag(editedTagData);
			else if (tagData is ExistingTagData existingTagData)
				RemoveTag(dataSet.TagsLibrary, existingTagData);
			else if (tagData is NewTagData newTagData)
				CreateNewTag(dataSet.TagsLibrary, newTagData);
			else
				throw new ArgumentOutOfRangeException(nameof(tagData), tagData, null);
		}
	}

	private static void RemoveTag(TagsLibrary tagsLibrary, ExistingTagData existingTagData)
	{
		if (tagsLibrary is TagsLibrary<Tag> plainTagsLibrary)
			plainTagsLibrary.DeleteTag(existingTagData.Tag);
		else if (tagsLibrary is TagsLibrary<PoserTag> poserTagsLibrary)
		{
			poserTagsLibrary.DeleteTag((PoserTag)existingTagData.Tag);
		}
	}

	private static void EditTag(EditedTagData editedTagData)
	{
		var tag = editedTagData.Tag;
		tag.Name = editedTagData.Name;
		tag.Color = editedTagData.Color;
		if (tag is PoserTag poserTag)
			EditKeyPoints(poserTag, ((EditedPoserTagData)editedTagData).KeyPointTags);
	}

	private static void EditKeyPoints(PoserTag poserTag, IReadOnlyCollection<TagData> tagsData)
	{
		var orderedData = tagsData.Order(TagDataOperationPriorityComparer.Instance);
		foreach (var tagData in orderedData)
		{
			if (tagData is EditedTagData editedTagData)
				EditTag(editedTagData);
			else if (tagData is ExistingTagData existingTagData)
				poserTag.DeleteKeyPointTag(existingTagData.Tag);
			else if (tagData is NewTagData newTagData)
				CreateKeyPointTag(poserTag, newTagData);
			else
				throw new ArgumentOutOfRangeException(nameof(tagData));
		}
	}

	private static void CreateNewTag(TagsLibrary tagsLibrary, NewTagData tagData)
	{
		var tag = tagsLibrary.CreateTag(tagData.Name);
		tag.Color = tagData.Color;
		if (tag is PoserTag poserTag)
			CreateKeyPointTags(poserTag, (NewPoserTagData)tagData);
	}

	private static void CreateKeyPointTags(PoserTag tag, NewPoserTagData data)
	{
		foreach (var keyPointTagData in data.KeyPointTags)
		{
			var keyPointTag = tag.CreateKeyPointTag(keyPointTagData.Name);
			keyPointTag.Color = keyPointTagData.Color;
		}
	}

	private static void CreateKeyPointTag(PoserTag poserTag, NewTagData data)
	{
		var keyPointTag = poserTag.CreateKeyPointTag(data.Name);
		keyPointTag.Color = data.Color;
	}
}