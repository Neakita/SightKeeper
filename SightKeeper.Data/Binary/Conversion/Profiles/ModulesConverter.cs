using System.Collections.Immutable;
using SightKeeper.Data.Binary.Profiles.Modules;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class ModulesConverter
{
	public ModulesConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_classifierConverter = new ClassifierModulesConverter(weightsDataAccess);
		_detectorConverter = new DetectorModulesConverter(weightsDataAccess);
		_poserConverter = new PoserModulesConverter(weightsDataAccess);
	}

	public ImmutableArray<SerializableModule> Convert(IEnumerable<Module> modules, ConversionSession session)
	{
		return modules.Select(module => Convert(module, session)).ToImmutableArray();
	}

	private SerializableModule Convert(Module module, ConversionSession session)
	{
		return module switch
		{
			ClassifierModule classifierModule => _classifierConverter.Convert(classifierModule, session),
			DetectorModule detectorModule => _detectorConverter.Convert(detectorModule, session),
			PoserModule poserModule => _poserConverter.Convert(poserModule, session),
			_ => throw new ArgumentOutOfRangeException(nameof(module))
		};
	}

	private readonly ClassifierModulesConverter _classifierConverter;
	private readonly PoserModulesConverter _poserConverter;
	private readonly DetectorModulesConverter _detectorConverter;
}