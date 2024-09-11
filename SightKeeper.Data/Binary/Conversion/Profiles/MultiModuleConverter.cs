using SightKeeper.Data.Binary.Model.Profiles.Modules;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Data.Binary.Conversion.Profiles;

internal sealed class MultiModuleConverter
{
	public MultiModuleConverter(ConversionSession session)
	{
		_classifierConverter = new ClassifierModuleConverter(session);
		_detectorConverter = new DetectorModuleConverter(session);
		_poser2DConverter = new Poser2DModuleConverter(session);
		_poser3DConverter = new Poser3DModuleConverter(session);
	}

	public IEnumerable<PackableModule> Convert(IEnumerable<Module> modules)
	{
		return modules.Select(Convert);
	}

	private PackableModule Convert(Module module) => module switch
	{
		ClassifierModule classifierModule => _classifierConverter.Convert(classifierModule),
		DetectorModule detectorModule => _detectorConverter.Convert(detectorModule),
		Poser2DModule poser2DModule => _poser2DConverter.Convert(poser2DModule),
		Poser3DModule poser3DModule => _poser3DConverter.Convert(poser3DModule),
		_ => throw new ArgumentOutOfRangeException(nameof(module))
	};

	private readonly ClassifierModuleConverter _classifierConverter;
	private readonly DetectorModuleConverter _detectorConverter;
	private readonly Poser2DModuleConverter _poser2DConverter;
	private readonly Poser3DModuleConverter _poser3DConverter;
}