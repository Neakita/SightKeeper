using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model;
using SightKeeper.Data.Binary.Replication.DataSets;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Replication;

internal static class GameReplicator
{
	public static HashSet<Game> Replicate(IEnumerable<PackableGame> packedGames, ReplicationSession session)
	{
		var lookupBuilder = ImmutableDictionary.CreateBuilder<ushort, Game>();
		HashSet<Game> games = new();
		foreach (var packedGame in packedGames)
		{
			Game game = new(packedGame.Title, packedGame.ProcessName, packedGame.ExecutablePath);
			games.Add(game);
			lookupBuilder.Add(packedGame.Id, game);
		}
		session.Games = lookupBuilder.ToImmutable();
		return games;
	}
}