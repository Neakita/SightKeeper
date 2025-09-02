using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.DataSets.Card;

namespace SightKeeper.Avalonia.DataSets;

internal sealed class DesignDataSetsDataContext : DataSetsDataContext
{
	public IReadOnlyCollection<DataSetCardDataContext> DataSets { get; } =
	[
		new DesignDataSetCardDataContext("PD2"),
		new DesignDataSetCardDataContext("KF2 Sample 1", "kfSample1.jpg"),
		new DesignDataSetCardDataContext("KF2 Sample 2", "kfSample2.jpg"),
		new DesignDataSetCardDataContext("KF2 Sample 3", "kfSample3.jpg"),
		new DesignDataSetCardDataContext("KF2 Sample 4", "kfSample4.jpg"),
		new DesignDataSetCardDataContext("KF2 Sample 5", "kfSample5.jpg"),
		new DesignDataSetCardDataContext("KF2 Sample 6", "kfSample6.jpg"),
		new DesignDataSetCardDataContext("KF2 Sample 7", "kfSample7.jpg"),
		new DesignDataSetCardDataContext("KF2 Sample 8", "kfSample8.jpg"),
	];

	public ICommand CreateDataSetCommand { get; } = new RelayCommand(() => { });
	public ICommand ImportDataSetCommand { get; } = new RelayCommand(() => { });
}