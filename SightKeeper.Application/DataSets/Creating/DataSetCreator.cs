using FluentValidation;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Application.DataSets.Creating;

public sealed class DataSetCreator
{
	public required IValidator<NewDataSetData> Validator { get; init; }
	public required WriteRepository<DataSet> Repository { get; init; }

	public DataSet Create(NewDataSetData data)
	{
		Validator.ValidateAndThrow(data);
		DataSet dataSet = data.Type switch
		{
			DataSetType.Classifier => new DomainClassifierDataSet(),
			DataSetType.Detector => new DomainDetectorDataSet(),
			DataSetType.Poser2D => new Poser2DDataSet(),
			DataSetType.Poser3D => new Poser3DDataSet(),
			_ => throw new ArgumentOutOfRangeException(nameof(data), data, null)
		};
		SetGeneralData(dataSet, data);
		AddTags(dataSet, data.NewTags);
		Repository.Add(dataSet);
		return dataSet;
	}

	private static void SetGeneralData(DataSet dataSet, DataSetData data)
	{
		dataSet.Name = data.Name;
		dataSet.Description = data.Description;
	}

	private static void AddTags(DataSet dataSet, IEnumerable<NewTagData> tagsData)
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