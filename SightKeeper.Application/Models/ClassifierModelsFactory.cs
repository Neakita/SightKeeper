using SightKeeper.Domain.Model.Classifier;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Models;

public sealed class ClassifierModelsFactory : IModelsFactory<ClassifierModel>
{
	public ClassifierModel Create(string name, Resolution resolution) => new(name, resolution);
}
