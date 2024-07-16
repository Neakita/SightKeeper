using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Conversion;

internal static class GamesConverter
{
	public static ImmutableArray<SerializableGame> Convert(IReadOnlyCollection<Game> games, ConversionSession session)
	{
		var converted = games.Select((game, index) => new SerializableGame((ushort)index, game)).ToImmutableArray();
		session.Games = games.Zip(converted).ToImmutableDictionary(t => t.First, t => t.Second.Id);
		return converted;
	}

	public static HashSet<Game> ConvertBack(
		ImmutableArray<SerializableGame> games,
		ReverseConversionSession session)
	{
		var converted = games.Select(game => game.ToGame());
		session.Games = games.Zip(converted).ToImmutableDictionary(t => t.First.Id, t => t.Second);
		return session.Games.Values.ToHashSet();
	}

	public static ushort? GetGameId(Game? game, ConversionSession session)
	{
		Guard.IsNotNull(session.Games);
		if (game == null)
			return null;
		return session.Games[game];
	}
}