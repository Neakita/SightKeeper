using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Models;

public sealed class DetectorModelsFactory : IModelsFactory<DetectorModel>
{
	public DetectorModel Create(string name, Resolution resolution) => new(name, resolution);
}
