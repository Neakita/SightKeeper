using FluentValidation;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Application.DataSets.Creating;

public sealed class DataSetCreator
{
	public required IValidator<NewDataSetData> Validator { get; init; }
	public required WriteRepository<DataSet<Asset>> Repository { get; init; }
	public required DataSetFactory<ClassifierAsset> ClassifierFactory { get; init; }
	public required DataSetFactory<ItemsAsset<DetectorItem>> DetectorFactory { get; init; }
	public required DataSetFactory<ItemsAsset<PoserItem>> PoserFactory { get; init; }

	public DataSet<Asset> Create(NewDataSetData data)
	{
		Validator.ValidateAndThrow(data);
		var dataSet = CreateDataSet(data.Type);
		SetGeneralData(dataSet, data);
		AddTags(dataSet, data.NewTags);
		Repository.Add(dataSet);
		return dataSet;
	}

	private DataSet<Asset> CreateDataSet(DataSetType type)
	{
		DataSetFactory<Asset> factory = type switch
		{
			DataSetType.Classifier => ClassifierFactory,
			DataSetType.Detector => DetectorFactory,
			DataSetType.Poser => PoserFactory,
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
		};
		return factory.CreateDataSet();
	}

	private static void SetGeneralData(DataSet<Asset> dataSet, DataSetData data)
	{
		dataSet.Name = data.Name;
		dataSet.Description = data.Description;
	}

	private static void AddTags(DataSet<Asset> dataSet, IEnumerable<NewTagData> tagsData)
	{
		foreach (var tagData in tagsData)
		{
			var tag = dataSet.TagsLibrary.CreateTag(tagData.Name);
			tag.Color = tagData.Color;
			if (tag is DomainPoserTag poserTag)
				CreateKeyPointTags(poserTag, (NewPoserTagData)tagData);
		}
	}

	private static void CreateKeyPointTags(DomainPoserTag tag, NewPoserTagData data)
	{
		foreach (var keyPointTagData in data.KeyPointTags)
		{
			var keyPointTag = tag.CreateKeyPointTag(keyPointTagData.Name);
			keyPointTag.Color = keyPointTagData.Color;
		}
	}
}