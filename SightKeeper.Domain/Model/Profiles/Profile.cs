using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Domain.Model.Profiles;

public sealed class Profile
{
	public string Name { get; set; }
	public IReadOnlyCollection<Module> Modules => _modules;

	public Profile(string name)
	{
		Name = name;
	}

	public DetectorModule CreateModule(DetectorWeights weights)
	{
		DetectorModule module = new(this, weights);
		_modules.Add(module);
		return module;
	}

	public ClassifierModule CreateModule(ClassifierWeights weights)
	{
		ClassifierModule module = new(this, weights);
		_modules.Add(module);
		return module;
	}

	public PoserModule CreateModule(Poser2DWeights weights)
	{
		PoserModule module = new(this, weights);
		_modules.Add(module);
		return module;
	}

	public void RemoveModule(Module module)
	{
		_modules.Remove(module);
	}
	
	private readonly HashSet<Module> _modules = new();
}