using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.Profiles;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class ProfileConverter
{
	public IEnumerable<PackableProfile> Convert(IEnumerable<Profile> profiles, ConversionSession session)
	{
		return profiles.Select(profile => Convert(profile, session));
	}

	private PackableProfile Convert(Profile profile, ConversionSession session)
	{
		var modules = _modulesConverter.Convert(profile.Modules, session).ToImmutableArray();
		return new PackableProfile(profile.Name, profile.Description, modules);
	}

	private readonly MultiModuleConverter _modulesConverter = new();
}