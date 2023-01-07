using SightKeeper.DAL.Domain.Classifier;
using SightKeeper.DAL.Domain.Common;

namespace SightKeeper.Backend.Models;

public sealed class ClassifierModelsFactory : IModelsFactory<ClassifierModel>
{
	public ClassifierModel Create(string name, Resolution resolution) => new(name, resolution);
}
