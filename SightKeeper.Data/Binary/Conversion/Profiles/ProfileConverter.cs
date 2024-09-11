using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.Profiles;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class ProfileConverter
{
	public ProfileConverter(ConversionSession session)
	{
		_modulesConverter = new MultiModuleConverter(session);
	}

	public IEnumerable<PackableProfile> Convert(IEnumerable<Profile> profiles)
	{
		return profiles.Select(Convert);
	}

	private PackableProfile Convert(Profile profile)
	{
		var modules = _modulesConverter.Convert(profile.Modules).ToImmutableArray();
		return new PackableProfile(profile.Name, profile.Description, modules);
	}

	private readonly MultiModuleConverter _modulesConverter;
}