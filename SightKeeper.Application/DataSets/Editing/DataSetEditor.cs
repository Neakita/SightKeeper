using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Editing;

public class DataSetEditor : IDisposable
{
	public IObservable<DataSet> DataSetEdited => _dataSetEdited.AsObservable();

	public virtual void Edit(DataSet dataSet, DataSetData data)
	{
		SetGeneralData(dataSet, data);
		UpdateTags(dataSet, data.TagsChanges);
		_dataSetEdited.OnNext(dataSet);
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

	private static void UpdateTags(DataSet dataSet, TagsChanges changes)
	{
		foreach (var tag in changes.RemovedTags)
			RemoveTag(dataSet.TagsLibrary, tag);
		foreach (var tag in changes.EditedTags)
			EditTag(tag);
		foreach (var tag in changes.NewTags)
			CreateNewTag(dataSet.TagsLibrary, tag);
	}

	private static void RemoveTag(TagsLibrary tagsLibrary, Tag tag)
	{
		if (tagsLibrary is TagsLibrary<Tag> plainTagsLibrary)
			plainTagsLibrary.DeleteTag(tag);
		else if (tagsLibrary is TagsLibrary<PoserTag> poserTagsLibrary)
			poserTagsLibrary.DeleteTag((PoserTag)tag);
	}

	private static void EditTag(EditedTagData editedTagData)
	{
		var tag = editedTagData.Tag;
		tag.Name = editedTagData.Name;
		tag.Color = editedTagData.Color;
		if (tag is PoserTag poserTag)
		{
			var editedPoserTagData = (EditedPoserTagData)editedTagData;
			EditKeyPoints(poserTag, editedPoserTagData.KeyPointTagsChanges);
		}
	}

	private static void EditKeyPoints(PoserTag poserTag, TagsChanges changes)
	{
		foreach (var tag in changes.RemovedTags)
			poserTag.DeleteKeyPointTag(tag);
		foreach (var tag in changes.EditedTags)
			EditTag(tag);
		foreach (var tag in changes.NewTags)
			CreateKeyPointTag(poserTag, tag);
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