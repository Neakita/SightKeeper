using System.Collections.Immutable;
using SightKeeper.Data.Binary.Profiles;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class ProfilesConverter
{
	public ProfilesConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_modulesConverter = new ModulesConverter(weightsDataAccess);
	}
	
	public ImmutableArray<SerializableProfile> Convert(IEnumerable<Profile> profiles, ConversionSession session)
	{
		return profiles.Select(profile => Convert(profile, session)).ToImmutableArray();
	}

	private SerializableProfile Convert(Profile profile, ConversionSession session)
	{
		var modules = _modulesConverter.Convert(profile.Modules, session);
		return new SerializableProfile(profile.Name, modules);
	}

	private readonly ModulesConverter _modulesConverter;
}