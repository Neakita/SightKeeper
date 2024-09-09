using SightKeeper.Data.Binary.Model.Profiles.Modules;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class MultiModuleConverter
{
	public MultiModuleConverter()
	{
		_classifierConverter = new ClassifierModuleConverter();
		_detectorConverter = new DetectorModuleConverter();
		_poser2DConverter = new Poser2DModuleConverter();
		_poser3DConverter = new Poser3DModuleConverter();
	}

	public IEnumerable<PackableModule> Convert(IEnumerable<Module> modules, ConversionSession session)
	{
		return modules.Select(module => Convert(module, session));
	}

	private PackableModule Convert(Module module, ConversionSession session) => module switch
	{
		ClassifierModule classifierModule => _classifierConverter.Convert(classifierModule, session),
		DetectorModule detectorModule => _detectorConverter.Convert(detectorModule, session),
		Poser2DModule poser2DModule => _poser2DConverter.Convert(poser2DModule, session),
		Poser3DModule poser3DModule => _poser3DConverter.Convert(poser3DModule, session),
		_ => throw new ArgumentOutOfRangeException(nameof(module))
	};

	private readonly ClassifierModuleConverter _classifierConverter;
	private readonly DetectorModuleConverter _detectorConverter;
	private readonly Poser2DModuleConverter _poser2DConverter;
	private readonly Poser3DModuleConverter _poser3DConverter;
}