using FluentValidation;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Creating;

public sealed class DataSetCreator
{
	public required IValidator<NewDataSetData> Validator { get; init; }
	public required WriteRepository<DataSet<Tag, Asset>> Repository { get; init; }
	public required DataSetFactory<Tag, ClassifierAsset> ClassifierFactory { get; init; }
	public required DataSetFactory<Tag, ItemsAsset<DetectorItem>> DetectorFactory { get; init; }
	public required DataSetFactory<PoserTag, ItemsAsset<PoserItem>> PoserFactory { get; init; }

	public DataSet<Tag, Asset> Create(NewDataSetData data)
	{
		Validator.ValidateAndThrow(data);
		var dataSet = CreateDataSet(data.Type);
		SetGeneralData(dataSet, data);
		AddTags(dataSet, data.NewTags);
		Repository.Add(dataSet);
		return dataSet;
	}

	private DataSet<Tag, Asset> CreateDataSet(DataSetType type)
	{
		DataSetFactory<Tag, Asset> factory = type switch
		{
			DataSetType.Classifier => ClassifierFactory,
			DataSetType.Detector => DetectorFactory,
			DataSetType.Poser => PoserFactory,
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
		};
		return factory.CreateDataSet();
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