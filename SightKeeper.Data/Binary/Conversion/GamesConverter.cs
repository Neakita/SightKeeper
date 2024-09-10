using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Conversion;

internal static class GamesConverter
{
	public static ImmutableArray<PackableGame> Convert(IReadOnlyCollection<Game> games, ConversionSession session)
	{
		var lookupBuilder = ImmutableDictionary.CreateBuilder<Game, ushort>();
		var resultBuilder = ImmutableArray.CreateBuilder<PackableGame>(games.Count);
		ushort gameIndex = 0;
		foreach (var game in games)
		{
			var gameId = gameIndex++;
			lookupBuilder.Add(game, gameId);
			resultBuilder.Add(Convert(gameId, game));
		}
		session.GameIds = lookupBuilder.ToImmutable();
		return resultBuilder.DrainToImmutable();
	}

	private static PackableGame Convert(ushort id, Game game)
	{
		return new PackableGame(id, game.Title, game.ProcessName, game.ExecutablePath);
	}
}