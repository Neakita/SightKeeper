using System.Reactive.Linq;
using System.Reactive.Subjects;
using FluentValidation;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Editing;

public class DataSetEditor : IDisposable
{
	public required IValidator<ExistingDataSetData> Validator { get; init; }

	public IObservable<DataSet> DataSetEdited => _dataSetEdited.AsObservable();

	public virtual void Edit(ExistingDataSetData data)
	{
		var dataSet = data.DataSet;
		Validator.ValidateAndThrow(data);
		SetGeneralData(dataSet, data);
		UpdateTags(dataSet.TagsLibrary, data.TagsChanges);
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

	private static void UpdateTags(TagsOwner<Tag> tagsOwner, TagsChanges changes)
	{
		foreach (var tag in changes.RemovedTags)
			RemoveTag(tagsOwner, tag);
		foreach (var tag in changes.EditedTags)
			EditTag(tag);
		foreach (var tag in changes.NewTags)
			CreateNewTag(tagsOwner, tag);
	}

	private static void RemoveTag(TagsOwner<Tag> tagsOwner, Tag tag)
	{
		var tagIndex = tagsOwner.Tags.Index().First(t => t.Item == tag).Index;
		tagsOwner.DeleteTagAt(tagIndex);
	}

	private static void EditTag(EditedTagData editedTagData)
	{
		var tag = editedTagData.Tag;
		tag.Name = editedTagData.Name;
		tag.Color = editedTagData.Color;
		if (tag is PoserTag poserTag)
		{
			var editedPoserTagData = (EditedPoserTagData)editedTagData;
			UpdateTags(poserTag, editedPoserTagData.KeyPointTagsChanges);
		}
	}

	private static void CreateNewTag(TagsOwner<Tag> tagsLibrary, NewTagData tagData)
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
}