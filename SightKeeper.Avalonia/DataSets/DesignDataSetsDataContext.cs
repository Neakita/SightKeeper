using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.DataSets;

internal sealed class DesignDataSetsDataContext : DataSetsDataContext
{
	public IReadOnlyCollection<DataSetViewModel> DataSets { get; } =
	[
		new(new ClassifierDataSet { Name = "Classifier DataSet" }),
		new(new DetectorDataSet { Name = "Detector DataSet" }),
		new(new Poser2DDataSet { Name = "Poser 2D DataSet" }),
		new(new Poser3DDataSet { Name = "Poser 3D DataSet" }),
	];

	public ICommand CreateDataSetCommand { get; } = new RelayCommand(() => { });
	public ICommand EditDataSetCommand { get; } = new RelayCommand(() => { });
	public ICommand DeleteDataSetCommand { get; } = new RelayCommand(() => { });
}