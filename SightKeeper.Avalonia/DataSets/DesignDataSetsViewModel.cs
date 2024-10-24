using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Avalonia.DataSets;

internal sealed class DesignDataSetsViewModel : IDataSetsViewModel
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