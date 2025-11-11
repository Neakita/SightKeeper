using FluentValidation;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Application.Misc;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Creating;

public sealed class DataSetCreator(IValidator<NewDataSetData> validator, WriteRepository<DataSet<Tag, Asset>> repository)
{
	public DataSet<Tag, Asset> Create(NewDataSetData data)
	{
		validator.ValidateAndThrow(data);
		var dataSet = data.DataSetFactory.Create();
		SetGeneralData(dataSet, data);
		AddTags(dataSet, data.NewTags);
		repository.Add(dataSet);
		return dataSet;
	}

	private static void SetGeneralData(DataSet<Tag, Asset> dataSet, DataSetData data)
	{
		dataSet.Name = data.Name;
		dataSet.Description = data.Description;
	}

	private static void AddTags(DataSet<Tag, Asset> dataSet, IEnumerable<NewTagData> tagsData)
	{
		foreach (var tagData in tagsData)
		{
			var tag = dataSet.TagsLibrary.CreateTag(tagData.Name);
			tag.Color = tagData.Color;
			if (tag is PoserTag poserTag)
				CreateKeyPointTags(poserTag, (NewPoserTagData)tagData);
		}
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