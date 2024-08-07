using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Data.Binary.Conversion;

internal static class GamesConverter
{
	public static ImmutableArray<Game> Convert(IReadOnlyCollection<Domain.Model.Game> games, ConversionSession session)
	{
		var converted = games.Select((game, index) => new Game((ushort)index, game)).ToImmutableArray();
		session.Games = games.Zip(converted).ToImmutableDictionary(t => t.First, t => t.Second.Id);
		return converted;
	}

	public static HashSet<Domain.Model.Game> ConvertBack(
		ImmutableArray<Game> games,
		ReverseConversionSession session)
	{
		var converted = games.Select(game => game.ToGame());
		session.Games = games.Zip(converted).ToImmutableDictionary(t => t.First.Id, t => t.Second);
		return session.Games.Values.ToHashSet();
	}

	public static ushort? GetGameId(Domain.Model.Game? game, ConversionSession session)
	{
		Guard.IsNotNull(session.Games);
		if (game == null)
			return null;
		return session.Games[game];
	}
}