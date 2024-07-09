using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary.Formatters;

public sealed class AppDataFormatter : MemoryPackFormatter<AppData>
{
	public AppDataFormatter(FileSystemScreenshotsDataAccess screenshotsDataAccess, FileSystemDetectorWeightsDataAccess weightsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_weightsDataAccess = weightsDataAccess;
	}
	
	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref AppData? value)
	{
		if (value == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}

		var games = value.Games.Select((game, index) => (game, index)).ToDictionary(tuple => tuple.game,
			tuple => new SerializableGame((ushort)tuple.index, tuple.game));
		var detectorDataSets = value.DetectorDataSets.Select(dataSet => Convert(dataSet, games)).ToImmutableArray();
		RawAppData raw = new(detectorDataSets, games.Values.ToImmutableArray());
		writer.WritePackable(raw);
	}

	public override void Deserialize(ref MemoryPackReader reader, scoped ref AppData? value)
	{
		if (reader.PeekIsNull())
		{
			reader.Advance(1); // skip null block
			value = null;
			return;
		}

		var raw = reader.ReadPackable<RawAppData>();
		Guard.IsNotNull(raw);
		var games = raw.Games.ToDictionary(game => game.Id, game => new Game(game.Title, game.ProcessName, game.ExecutablePath));
		var detectorDataSets = Convert(raw.DetectorDataSets, gameId => games[gameId]);
		value = new AppData(detectorDataSets.ToHashSet(), games.Values.ToHashSet());
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
	private readonly FileSystemDetectorWeightsDataAccess _weightsDataAccess;

	private SerializableDetectorDataSet Convert(
		DetectorDataSet dataSet,
		IReadOnlyDictionary<Game, SerializableGame> games)
	{
		ushort? gameId = dataSet.Game == null ? null : games[dataSet.Game].Id;
		var tagsDict = Convert(dataSet.Tags);
		var screenshots = Convert(dataSet.Screenshots).ToImmutableArray();
		var assets = Convert(dataSet.Assets, tag => tagsDict[tag].Id).ToImmutableArray();
		var weights = Convert(dataSet.Weights, tag => tagsDict[tag].Id).ToImmutableArray();
		SerializableDetectorDataSet serializableDataSet = new(
			dataSet.Name,
			dataSet.Description,
			gameId,
			dataSet.Resolution,
			dataSet.Screenshots.MaxQuantity,
			tagsDict.Values,
			assets,
			weights,
			screenshots
		);
		return serializableDataSet;
	}

	private static Dictionary<Tag, SerializableTag> Convert(IEnumerable<Tag> tags)
	{
		return tags.ToDictionary(tag => tag, tag => new SerializableTag(Id.Create(), tag.Name, tag.Color));
	}

	private IEnumerable<SerializableScreenshot> Convert(IEnumerable<Screenshot> screenshots)
	{
		foreach (var screenshot in screenshots)
			yield return new SerializableScreenshot(_screenshotsDataAccess.GetId(screenshot), screenshot.CreationDate);
	}

	private IEnumerable<SerializableDetectorAsset> Convert(IEnumerable<DetectorAsset> assets, Func<DetectorTag, Id> getTagId)
	{
		foreach (var asset in assets)
			yield return new SerializableDetectorAsset(
				_screenshotsDataAccess.GetId(asset.Screenshot),
				asset.Usage,
				Convert(asset.Items, getTagId).ToImmutableArray());
	}

	private static IEnumerable<SerializableDetectorItem> Convert(IEnumerable<DetectorItem> items, Func<DetectorTag, Id> getId)
	{
		foreach (var item in items)
			yield return new SerializableDetectorItem(getId(item.Tag), item.Bounding);
	}

	private IEnumerable<SerializableDetectorWeights> Convert(IEnumerable<DetectorWeights> weights, Func<DetectorTag, Id> getTagId)
	{
		foreach (var item in weights)
			yield return new SerializableDetectorWeights(
				_weightsDataAccess.GetId(item),
				item.CreationDate,
				item.Size,
				item.Metrics,
				item.Tags.Select(getTagId).ToImmutableArray());
	}

	private IEnumerable<DetectorDataSet> Convert(IEnumerable<SerializableDetectorDataSet> dataSets, Func<ushort, Game> getGame)
	{
		foreach (var raw in dataSets)
		{
			DetectorDataSet dataSet = new(raw.Name, raw.Resolution);
			if (raw.GameId != null)
				dataSet.Game = getGame(raw.GameId.Value);
			dataSet.Screenshots.MaxQuantity = raw.MaxScreenshots;
			dataSet.Description = raw.Description;
			Dictionary<Id, DetectorTag> tagsDict = new();
			foreach (var rawTag in raw.Tags)
			{
				var tag = dataSet.Tags.CreateTag(rawTag.Name);
				tagsDict.Add(rawTag.Id, tag);
				tag.Color = rawTag.Color;
			}
			Dictionary<Id, DetectorScreenshot> screenshotsDict = new();
			foreach (var rawScreenshot in raw.Screenshots)
			{
				var screenshot = CreateScreenshot(dataSet.Screenshots);
				CreationDateBackingField(screenshot) = rawScreenshot.CreationDate;
				screenshotsDict.Add(rawScreenshot.Id, screenshot);
				_screenshotsDataAccess.AssociateId(screenshot, rawScreenshot.Id);
			}
			foreach (var rawAsset in raw.Assets)
			{
				var screenshot = screenshotsDict[rawAsset.ScreenshotId];
				var asset = dataSet.Assets.MakeAsset(screenshot);
				foreach (var rawItem in rawAsset.Items)
					asset.CreateItem(tagsDict[rawItem.TagId], rawItem.Bounding);
			}
			foreach (var rawWeights in raw.Weights)
			{
				var weights = CreateWeights(dataSet.Weights, rawWeights.Size, rawWeights.Metrics, rawWeights.Tags.Select(tagId => tagsDict[tagId]));
				_weightsDataAccess.AssociateId(weights, rawWeights.Id);
			}
			yield return dataSet;
		}

		[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "CreateScreenshot")]
		static extern DetectorScreenshot CreateScreenshot(DetectorScreenshotsLibrary library);

		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<CreationDate>k__BackingField")]
		static extern ref DateTime CreationDateBackingField(Screenshot screenshot);
		
		[UnsafeAccessor(UnsafeAccessorKind.Method)]
		static extern DetectorWeights CreateWeights(DetectorWeightsLibrary library, ModelSize size, WeightsMetrics metrics, IEnumerable<DetectorTag> tags);
	}
}