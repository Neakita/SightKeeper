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
	public DataSetCreator(WriteRepository<DataSet> repository)
	{
		_repository = repository;
	}

	public DataSet Create(DataSetData data, DataSetType type)
	{
		DataSet dataSet = type switch
		{
			DataSetType.Classifier => new ClassifierDataSet(),
			DataSetType.Detector => new DetectorDataSet(),
			DataSetType.Poser2D => new Poser2DDataSet(),
			DataSetType.Poser3D => new Poser3DDataSet(),
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
		};
		SetGeneralData(dataSet, data);
		AddTags(dataSet, data.TagsChanges.NewTags);
		_repository.Add(dataSet);
		return dataSet;
	}

	private readonly WriteRepository<DataSet> _repository;

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