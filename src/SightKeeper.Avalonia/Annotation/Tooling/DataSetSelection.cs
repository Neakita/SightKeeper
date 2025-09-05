using System;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface DataSetSelection
{
	DataSet<Tag, Asset>? SelectedDataSet { get; }
	IObservable<DataSet<Tag, Asset>?> SelectedDataSetChanged { get; }
}