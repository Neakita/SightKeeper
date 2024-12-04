﻿using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class DataSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	public abstract TagsLibrary TagsLibrary { get; }
	public abstract AssetsLibrary AssetsLibrary { get; }
	public abstract WeightsLibrary WeightsLibrary { get; }
}