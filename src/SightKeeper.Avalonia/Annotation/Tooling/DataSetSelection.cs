using System;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface DataSetSelection
{
	DataSet? SelectedDataSet { get; }
	IObservable<DataSet?> SelectedDataSetChanged { get; }
}