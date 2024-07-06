using FluentAssertions;
using MemoryPack;
using SightKeeper.Data.Binary;
using SightKeeper.Data.Binary.Formatters;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Tests.Binary;

public sealed class BinarySerializationTests
{
	[Fact]
	public void ShouldSerializeAndDeserializeRawData()
	{
		RawAppData data = new([new SerializableDetectorDataSet("Test1", "", null, 160, null, [], [], [], [])], []);
		var serializedData = MemoryPackSerializer.Serialize(data);
		var deserializedData = MemoryPackSerializer.Deserialize<RawAppData>(serializedData);
		deserializedData.Should().BeEquivalentTo(data);
	}

	[Fact]
	public void ShouldSaveAndLoadAppData()
	{
		AppDataAccess dataAccess = new();
		MemoryPackFormatterProvider.Register(new AppDataFormatter(new FileSystemScreenshotsDataAccess(), new FileSystemDetectorWeightsDataAccess()));
		Game game = new("PayDay 2", "payday2");
		dataAccess.Data.Games.Add(game);
		DetectorDataSet dataSet = new("PD2", 320);
		dataSet.Description = "Test dataset named";
		dataSet.Game = game;
		var copTag = dataSet.Tags.CreateTag("Cop");
		copTag.Color = 123;
		var bulldozerTag = dataSet.Tags.CreateTag("Bulldozer");
		bulldozerTag.Color = 456;
		dataAccess.Data.DetectorDataSets.Add(dataSet);
		dataAccess.Save();
		var data = dataAccess.Data;
		dataAccess.Load();
		dataAccess.Data.Should().NotBeSameAs(data);
		var loadedDataSet = dataAccess.Data.DetectorDataSets.Single();
		loadedDataSet.Name.Should().Be(dataSet.Name);
		loadedDataSet.Resolution.Should().Be(dataSet.Resolution);
		loadedDataSet.Description.Should().Be(dataSet.Description);
		dataAccess.Data.Games.Single().Should().BeEquivalentTo(game);
		game = dataAccess.Data.Games.Single();
		loadedDataSet.Game.Should().BeSameAs(game);
		var dataSetTags = (IReadOnlyCollection<DetectorTag>)loadedDataSet.Tags;
		var loadedCopTag = dataSetTags.First();
		loadedCopTag.Name.Should().Be(copTag.Name);
		loadedCopTag.Color.Should().Be(copTag.Color);
		var loadedBulldozerTag = dataSetTags.ElementAt(1);
		loadedBulldozerTag.Name.Should().Be(bulldozerTag.Name);
		loadedBulldozerTag.Color.Should().Be(bulldozerTag.Color);
	}
}