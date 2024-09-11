using SightKeeper.Data.Binary.Model.Profiles;
using SightKeeper.Data.Binary.Replication.Profiles.Modules;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary.Replication.Profiles;

internal class ProfileReplicator
{
	public ProfileReplicator(ReplicationSession session)
	{
		_moduleReplicator = new MultiModuleReplicator(session);
	}

	public IEnumerable<Profile> Replicate(IEnumerable<PackableProfile> profiles)
	{
		return profiles.Select(Replicate);
	}

	private Profile Replicate(PackableProfile packed)
	{
		Profile profile = new(packed.Name)
		{
			Description = packed.Description
		};
		_moduleReplicator.Replicate(profile, packed.Modules);
		return profile;
	}

	private readonly MultiModuleReplicator _moduleReplicator;
}