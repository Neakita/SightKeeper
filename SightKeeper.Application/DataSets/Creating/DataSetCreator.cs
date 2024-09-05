using CommunityToolkit.Diagnostics;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Application.DataSets.Creating;

public sealed class DataSetCreator
{
	public DataSetCreator(WriteDataAccess<DataSet> dataAccess)
	{
		_dataAccess = dataAccess;
	}

	public DataSet Create(DataSetData data, IReadOnlyCollection<NewTagData> tagsData, DataSetType type)
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
		AddTags(dataSet, tagsData);
		_dataAccess.Add(dataSet);
		return dataSet;
	}

	private readonly WriteDataAccess<DataSet> _dataAccess;

	private static void SetGeneralData(DataSet dataSet, DataSetData data)
	{
		dataSet.Name = data.Name;
		dataSet.Description = data.Description;
		Guard.IsNotNull(data.Resolution);
		dataSet.Resolution = (ushort)data.Resolution;
		dataSet.Composition = data.Composition;
		dataSet.Game = data.Game;
	}

	private static void AddTags(DataSet dataSet, IReadOnlyCollection<NewTagData> tagsData)
	{
		foreach (var tagData in tagsData)
		{
			var tag = dataSet.Tags.CreateTag(tagData.Name);
			tag.Color = tagData.Color;
			if (tag is PoserTag poserTag)
				CreateKeyPointTags(poserTag, (NewPoserTagData)tagData);
		}
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