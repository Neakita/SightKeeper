using System.Buffers;
using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary.Formatters;

public class DetectorDataSetFormatter : IMemoryPackFormatter<DetectorDataSet>
{
	public DetectorDataSetFormatter(ImmutableDictionary<Game, Id> gameIds)
	{
		_gameIds = gameIds;
	}

	public void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref DetectorDataSet? value) where TBufferWriter : IBufferWriter<byte>
	{
		if (value == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}

		SerializableDetectorDataSet serializableValue = new(
			value.Name,
			value.Description,
			GetGameId(value.Game),
			value.Resolution,
			ConvertTags(value.Tags).ToList(),
			ConvertScreenshots(value.Screenshots).ToList(),
			ConvertAssets(value.Assets).ToList(),
			ConvertWeights(value.Weights).ToList());

		writer.WritePackable(serializableValue);
	}

	public void Deserialize(ref MemoryPackReader reader, scoped ref DetectorDataSet? value)
	{
		if (reader.PeekIsNull())
		{
			reader.Advance(1); // skip null block
			value = null;
			return;
		}

		var raw = reader.ReadPackable<SerializableDetectorDataSet>();
		if (raw == null)
		{
			value = null;
			return;
		}

		value = new DetectorDataSet(raw.Name, raw.Resolution);
		throw new NotImplementedException();
	}

	private readonly ImmutableDictionary<Game, Id> _gameIds;
	private readonly ImmutableDictionary<Tag, Id> _tagIds = ImmutableDictionary<Tag, Id>.Empty;

	private Id? GetGameId(Game? game)
	{
		if (game == null)
			return null;
		return _gameIds[game];
	}

	private static IEnumerable<SerializableTag> ConvertTags(IEnumerable<Tag> tags)
	{
		var tagsBuilder = ImmutableDictionary.CreateBuilder<Tag, Id>();
		foreach (var tag in tags)
		{
			var id = Id.Create();
			tagsBuilder.Add(tag, id);
			yield return new SerializableTag(id, tag.Name, tag.Color);
		}
	}

	private static IEnumerable<Id> ConvertScreenshots(IEnumerable<Screenshot> screenshots)
	{
		throw new NotImplementedException();
	}

	private static IEnumerable<SerializableDetectorAsset> ConvertAssets(IEnumerable<DetectorAsset> assets)
	{
		throw new NotImplementedException();
	}

	private static IEnumerable<SerializableDetectorWeights> ConvertWeights(IEnumerable<DetectorWeights> weights)
	{
		throw new NotImplementedException();
	}
}