﻿using SightKeeper.Domain.Common;
using SightKeeper.Domain.Detector;

namespace SightKeeper.Backend.Models;

public sealed class DetectorModelsFactory : IModelsFactory<DetectorModel>
{
	public DetectorModel Create(string name, Resolution resolution) => new(name, resolution);
}
