using System;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface DataSetSelection
{
	DataSet<Asset>? SelectedDataSet { get; }
	IObservable<DataSet<Asset>?> SelectedDataSetChanged { get; }
}