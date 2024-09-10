using SightKeeper.Data.Binary.Model.Profiles;
using SightKeeper.Data.Binary.Replication.Profiles.Modules;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary.Replication.Profiles;

internal static class ProfileReplicator
{
	public static IEnumerable<Profile> Replicate(IEnumerable<PackableProfile> profiles, ReplicationSession session)
	{
		return profiles.Select(profile => Replicate(profile, session));
	}

	private static Profile Replicate(PackableProfile packed, ReplicationSession session)
	{
		Profile profile = new(packed.Name)
		{
			Description = packed.Description
		};
		MultiModuleReplicator.Replicate(profile, packed.Modules, session);
		return profile;
	}
}