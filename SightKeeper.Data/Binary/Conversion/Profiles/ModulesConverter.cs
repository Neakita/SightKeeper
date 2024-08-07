using System.Collections.Immutable;
using SightKeeper.Data.Binary.Services;
using ClassifierModule = SightKeeper.Domain.Model.Profiles.Modules.ClassifierModule;
using DetectorModule = SightKeeper.Domain.Model.Profiles.Modules.DetectorModule;
using Module = SightKeeper.Data.Binary.Profiles.Modules.Module;
using PoserModule = SightKeeper.Domain.Model.Profiles.Modules.PoserModule;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class ModulesConverter
{
	public ModulesConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_classifierConverter = new ClassifierModulesConverter(weightsDataAccess);
		_detectorConverter = new DetectorModulesConverter(weightsDataAccess);
		_poserConverter = new PoserModulesConverter(weightsDataAccess);
	}

	public ImmutableArray<Module> Convert(IEnumerable<Domain.Model.Profiles.Modules.Module> modules, ConversionSession session)
	{
		return modules.Select(module => Convert(module, session)).ToImmutableArray();
	}

	private Module Convert(Domain.Model.Profiles.Modules.Module module, ConversionSession session)
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