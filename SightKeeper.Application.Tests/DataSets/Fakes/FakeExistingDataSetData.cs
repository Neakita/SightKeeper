using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Tests.DataSets.Fakes;

internal sealed class FakeExistingDataSetData : ExistingDataSetData
{
	public static FakeExistingDataSetData CreateWithRemovedTags(DataSet dataSet, params IEnumerable<Tag> removedTags)
	{
		FakeTagsChanges tagsChanges = new()
		{
			RemovedTags = removedTags
		};
		return new FakeExistingDataSetData(dataSet, tagsChanges);
	}

	public static FakeExistingDataSetData CreateWithEditedTag(DataSet dataSet, Tag tag, string newName)
	{
		FakeEditedTagData editedTag = new(tag, newName);
		FakeTagsChanges tagsChanges = new()
		{
			EditedTags = [editedTag]
		};
		return new FakeExistingDataSetData(dataSet, tagsChanges);
	}

	public static FakeExistingDataSetData CreateWithNewTags(DataSet dataSet, params IEnumerable<string> newTagsNames)
	{
		FakeTagsChanges tagsChanges = new()
		{
			NewTags = newTagsNames.Select(name => new FakeNewTagData(name))
		};
		return new FakeExistingDataSetData(dataSet, tagsChanges);
	}

	public static FakeExistingDataSetData CreateWithNewPoserTag(PoserDataSet dataSet, string poserTagName, params IEnumerable<string> keyPointTagsNames)
	{
		var keyPointTags = keyPointTagsNames.Select(name => new FakeNewTagData(name));
		FakeNewPoserTagData poserTag = new(poserTagName, keyPointTags);
		FakeTagsChanges tagsChanges = new()
		{
			NewTags = [poserTag]
		};
		return new FakeExistingDataSetData(dataSet, tagsChanges);
	}

	public static FakeExistingDataSetData CreateWithRemovedKeyPointTag(PoserDataSet dataSet, PoserTag poserTag, Tag keyPointTag)
	{
		FakeTagsChanges keyPointTagsChanges = new()
		{
			RemovedTags = [keyPointTag]
		};
		FakeEditedPoserTag editedPoserTag = new(poserTag, keyPointTagsChanges);
		FakeTagsChanges tagsChanges = new()
		{
			EditedTags = [editedPoserTag]
		};
		return new FakeExistingDataSetData(dataSet, tagsChanges);
	}

	public string Name { get; }
	public string Description { get; }
	public TagsChanges TagsChanges { get; }
	public DataSet DataSet { get; }

	public FakeExistingDataSetData(DataSet dataSet, string name, string description)
	{
		DataSet = dataSet;
		Name = name;
		Description = description;
		TagsChanges = new FakeTagsChanges();
	}

	public FakeExistingDataSetData(DataSet dataSet, string name)
	{
		DataSet = dataSet;
		Name = name;
		Description = dataSet.Description;
		TagsChanges = new FakeTagsChanges();
	}

	private FakeExistingDataSetData(DataSet dataSet, TagsChanges tagsChanges)
	{
		DataSet = dataSet;
		Name = dataSet.Name;
		Description = dataSet.Description;
		TagsChanges = tagsChanges;
	}
}